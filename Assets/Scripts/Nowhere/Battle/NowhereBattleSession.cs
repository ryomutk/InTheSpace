using UnityEngine;
using Actor;
using System.Collections.Generic;
using CardSystem;

//Nowhere in the square用のバトルセッションプロトコォル
public class NowhereBattleSession : BattleSession
{
    int nowCharacterCount = 0;
    int defaultDuration;

    //これは全キャラクターのターンが終わるたび、一度呼ばれる。
    Dictionary<ITimerObject, int> timerObjectRemainingTime = new Dictionary<ITimerObject, int>();

    Dictionary<Character, int> remainingTimes = new Dictionary<Character, int>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loopDuration">これが0以下になるとあれ。</param>
    /// <param name="profile"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public NowhereBattleSession(int loopDuration, SessionProfile profile, BattleField field) : base(profile, field)
    {
        defaultDuration = loopDuration;
        BattleManager.instance.OnTurnEvent += (x) =>
        {
            TurnSequencer(x);
        };
    }

    public override Character AddCharacter(CharacterName name)
    {
        var instance = base.AddCharacter(name);
        remainingTimes[instance] = defaultDuration;


        return instance;
    }

    public float GetRemainTimeNormalized(Character character)
    {
        if (remainingTimes.TryGetValue(character, out int remain))
        {
            var num = remain / (float)defaultDuration;
            return num;
        }
        else
        {
            Debug.Log("you aready are dead");
            return 1;
        }
    }

    public void ModifyCoolTime(Character character, int amount)
    {
        remainingTimes[character] += amount;
    }


    void TurnSequencer(TurnState state)
    {

        if (nowCharacterCount == characterInBattle.Count)
        {
            nowCharacterCount = 0;
        }

        var chara = characterInBattle[nowCharacterCount];

        if (state == TurnState.turnStart)
        {
            //上書き。
            chara = characterInBattle[nowCharacterCount];

            //時計が動いていても行動するのは一人。
            TurnStartProcess(chara);

            //みんなの時計を動かす-> こうじゃないとシーン内の人数で速さが変わってしまう。
            for (int i = 0; i < characterInBattle.Count; i++)
            {
                var target = characterInBattle[i];
                MoveClock(target);
            }

        }
        else if (state == TurnState.cardDraw)
        {
            if (remainingTimes[chara] <= 0)
            {
                CardDrawProcess(chara);
                //これ判定も含めてCardDrawProcessの中でも良いかも？
                remainingTimes[chara] = defaultDuration;
            }
        }
        else if (state == TurnState.turnEnd)
        {
            nowCharacterCount++;
        }
    }

    void TurnStartProcess(Character chara)
    {
        if (PlayerManager.instance.IsPlayer(chara))
        {
            Hand.instance.ExecuteCard();

            //とりあえず今はロック解除。
            Hand.instance.locked = false;
        }
    }

    void MoveClock(Character target)
    {
        var remainTime = remainingTimes[target];

        remainTime -= target.Status.statusDictionary[CharacterStates.dexterity];
        remainingTimes[target] = remainTime;

    }


    /// <summary>
    /// キャラのクールダウンタイムが0になったときのプロセス
    /// </summary>
    /// <param name="chara"></param>
    void CardDrawProcess(Character chara)
    {
        var drawnCard = BattleManager.instance.deckHolder.GetWeightData(chara).DrawCard();

        //プレイヤーの場合は
        if (PlayerManager.instance.IsPlayer(chara))
        {
            var instance = CardServer.instance.GetCard(drawnCard);

            //Handに送る。
            CardRouter.instance.SetCard(Hand.instance, drawnCard);
        }
        else
        {
            var action = CardServer.instance.GetActionBase(drawnCard);
            var command = new CommandEffect(() => action.Execute(chara));
            BattleManager.instance.RegisterFX(command);
            //仮
            //action.Execute(chara);
        }
    }

    protected override void OnKilled(Character target)
    {
        remainingTimes.Remove(target);
        base.OnKilled(target);
    }

    void PlayerSequence()
    {
        throw new System.NotImplementedException();
    }

    public void RegisterTimerObject(ITimerObject obj)
    {
        timerObjectRemainingTime[obj] = defaultDuration;
    }

    public void DisregisterTimerObject(ITimerObject obj)
    {
        timerObjectRemainingTime.Remove(obj);
    }

    void UpdateTimerObjects()
    {
        foreach (var timerAndTime in timerObjectRemainingTime)
        {
            var remain = timerObjectRemainingTime[timerAndTime.Key];
            remain -= timerAndTime.Key.dexterity;

            if (remain <= 0)
            {
                timerAndTime.Key.NoticeTime();
                timerObjectRemainingTime[timerAndTime.Key] = defaultDuration;
            }
            else
            {
                timerObjectRemainingTime[timerAndTime.Key] = remain;
            }
        }
    }

    //これ以上前に進まない
    public float GetStepSize(Character target)
    {
        var field = this.field as NowhereBattleField;
        return field.GetStepSize(target);
    }

}