using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Utility;
using Actor;
using Effects;

public delegate void CharacterAction(Character target);


[RequireComponent(typeof(AudioSource), typeof(RendererGetter))]
public class BattleManager : Singleton<BattleManager>,IInteraptHandler
{
    [SerializeField]SessionProfile sampleProfile;
    List<IInteraptor> interaptorQueue = new List<IInteraptor>();
    VisualEffectQueue battleQueue;
    public NowhereBattleSession nowSession { get;private set; }
    public DeckHolder deckHolder{get;private set;}
    public PassiveHolder passiveHolder;

    /// <summary>
    /// TurnStateが投げられるEvent
    /// </summary>
    public event System.Action<TurnState> OnTurnEvent;

    /// <summary>
    /// BattleStateが投げられるEvent
    /// </summary>
    public event System.Action<BattleState> OnBattleEvent;
    BattleField field;


    void Start()
    {
       //     OnBattleEvent += (x) => Debug.Log("BatEv:"+x);
       //      OnTurnEvent += (x) => Debug.Log("TrnEv"+x);

        
        //これらMonobehaviourよりもきっとここにいるほうがいいと思ってる。いな？
        field = GetComponentInChildren<BattleField>();
        passiveHolder = new PassiveHolder();

        var audio = GetComponent<AudioSource>();
        var getter = GetComponent<RendererGetter>();
        battleQueue = new VisualEffectQueue(getter, audio, StartCoroutine);

        deckHolder = new DeckHolder();

        GameManager.instance.onGameEvent += (x) =>
        {
            if (x == GameState.inBattle)
            {
                StartCoroutine(BattleSequence());
            }
        };
    }

    
    //これはかくきゃらくたに設定され
    //毎ターンループ事(停止を覗いて訳10フレームほど)にDexだけ引かれる。
    //0以下になったら行動がとられる。
    [SerializeField] int loopDuration = 1000;
    IEnumerator BattleSequence()
    {
        nowSession = new NowhereBattleSession(loopDuration,sampleProfile,field);
        OnBattleEvent(BattleState.dataInitialize);
        yield return StartCoroutine(HandleInteraptors());
        OnBattleEvent(BattleState.viewInitialize);
        yield return StartCoroutine(HandleInteraptors());
        OnBattleEvent(BattleState.battleStart);
        yield return StartCoroutine(HandleInteraptors());

        StartCoroutine(TurnLoop(nowSession));
    }


    IEnumerator TurnLoop(BattleSession session)
    {
        TurnState[] states = System.Enum.GetValues(typeof(TurnState)) as TurnState[];

        while (!nowSession.IfBattleEnd)
        {
            for (int i = 0; i < states.Length; i++)
            {
                OnTurnEvent(states[i]);
                //入力、GUI待ち
                yield return null;
                battleQueue.Trigger();
                yield return StartCoroutine(HandleInteraptors());

                var additionalCount = battleQueue.Trigger();

                while (additionalCount > 0)
                {
                    yield return StartCoroutine(HandleInteraptors());
                    additionalCount = battleQueue.Trigger();
                }
            }
        }
    }

    public void RegisterFX(IVisualEffect effect)
    {
        battleQueue.RegisterFX(effect);
    }

        [Sirenix.OdinInspector.Button]
    void AddCharaTest(CharacterName name)
    {
        nowSession.AddCharacter(name);        
    }


    /// <param name="interaptor"></param>
    public void RegisterInterapt(IInteraptor interaptor)
    {
        interaptorQueue.Add(interaptor);
    }



    IEnumerator HandleInteraptors()
    {
        while (interaptorQueue.Count != 0)
        {
            yield return new WaitUntil(() => interaptorQueue[0].finished);
            interaptorQueue.RemoveAt(0);
        }
    }
}