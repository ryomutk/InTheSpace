using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Actor;
using Utility;

public class CharacterServer : Singleton<CharacterServer>, IInteraptor
{
    CharacterProfile[] characterProfiles;
    [SerializeField] Character rawCharacterPref;
    Utility.ObjPool.InstantPool<Character> characterPool;
    [SerializeField] int initNum = 10;

    //これはどのキャラにも使われるGUIであり
    //それらは再利用される。
    [SerializeField] TurnBaseUI[] additionalGUICloneBase;

    Dictionary<Character, List<TurnBaseUI>> characterGUI = new Dictionary<Character, List<TurnBaseUI>>();


    public bool finished { get; private set; }
    void Start()
    {
        GameManager.instance.onGameEvent += (x) =>
        {
            if (x == GameState.serverInitialize)
            {
                StartCoroutine(Initialize());
            };
        };
    }

    IEnumerator Initialize()
    {
        finished = false;
        GameManager.instance.RegisterInterapt(this);
        characterProfiles = CharacterProfileBuilder.GetAll();

        characterPool = new Utility.ObjPool.InstantPool<Character>(transform);
        yield return new WaitUntil(() => characterPool != null);
        characterPool.CreatePool(rawCharacterPref, initNum);
        yield return new WaitUntil(() => characterPool.state == ModuleState.ready);

        finished = true;
    }

    protected override void Awake()
    {
        base.Awake();
        characterProfiles = CharacterProfileBuilder.GetAll();
    }

    public Character GetCharacter(CharacterName name)
    {
        var profile = GetProfile(name);
        if (profile != null)
        {
            var chara = characterPool.GetObj();
            profile.LoadToCharacter(chara);
            SetGUI(chara);
            return chara;
        }

        return Instantiate(rawCharacterPref);
    }

    void SetGUI(Character charaIns)
    {
        //まだ装備していないなら
        if (!characterGUI.ContainsKey(charaIns))
        {
            characterGUI[charaIns] = new List<TurnBaseUI>();
            for (int i = 0; i < additionalGUICloneBase.Length; i++)
            {
                var gui = additionalGUICloneBase[i].Clone(charaIns);
                charaIns.Status.OnDeath += (x) => gui.DisActivate();
                characterGUI[charaIns].Add(gui);
            }
        }

        //目覚めさせる
        for(int i = 0;i < additionalGUICloneBase.Length;i++)
        {
            characterGUI[charaIns][i].Activate();
        }
    }

    public CharacterProfile GetProfile(CharacterName name)
    {
        for (int i = 0; i < characterProfiles.Length; i++)
        {
            if (characterProfiles[i].name == name)
            {
                return characterProfiles[i];
            }
        }

        return null;
    }


    public float GetCharacterDefault(CharacterName name, CharacterStates state)
    {
        var profile = GetProfile(name);
        for (int i = 0; i < profile.statesDataList.Count; i++)
        {
            if (profile.statesDataList[i].state == state)
            {
                return profile.statesDataList[i].amount;
            }
        }

        throw new System.Exception("Something wrong happened");
    }
}

