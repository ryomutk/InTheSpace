using UnityEngine;
using System.Collections.Generic;
using Utility.ObjPool;
using Utility;
using System.Collections;
using CardSystem;

public class CardServer : Singleton<CardServer>, IInteraptor
{
    List<CardActionBase> actionList = new List<CardActionBase>();
    CardViewProfile[] profiles;
    [SerializeField] Card rawCardPref;
    [SerializeField] int initNum = 10;
    InstantPool<Card> rawCardPool;
    public bool finished { get; private set; }

    void Start()
    {
        GameManager.instance.onGameEvent += (x) =>
        {
            if (x == GameState.serverInitialize)
            {
                StartCoroutine(Initialize());
            }
        };
    }

    IEnumerator Initialize()
    {
        finished = false;
        GameManager.instance.RegisterInterapt(this);

        profiles = CardProfileBuilder.GetAll();

        yield return null;

        rawCardPool = new InstantPool<Card>(transform);

        rawCardPool.CreatePool(rawCardPref, initNum);
        yield return new WaitUntil(() => rawCardPool.state == ModuleState.ready);


        var fileNames = System.IO.Directory.GetFiles(Application.dataPath + "/Resources/Scriptables/CardAction/", "*.asset");
        for (int i = 0; i < fileNames.Length; i++)
        {
            var filename = System.IO.Path.GetFileNameWithoutExtension(fileNames[i]);
            var request = Resources.LoadAsync("Scriptables/CardAction/" + filename);
            yield return new WaitUntil(() => request.isDone);
            actionList.Add(request.asset as CardActionBase);
        }

        finished = true;
    }

    public Card GetCard(CardName id)
    {
        var profile = GetProfile(id);
        var action = GetActionBase(id);
        if (profile != null && action != null)
        {
            var instance = rawCardPool.GetObj();
            instance.Initialize(profile, action);

            return instance;
        }
        //こいつは…バグカード的な?

        return Instantiate(rawCardPref);
    }

    public Card GetCard(CardViewProfile profile)
    {
        var action = GetActionBase(profile.name);
        if (action != null)
        {
            var instance = rawCardPool.GetObj();

            instance.Initialize(profile, action);

            return instance;
        }

        return Instantiate(rawCardPref);
    }


    public CardViewProfile GetProfile(CardName id)
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            if (profiles[i].name == id)
            {
                return profiles[i];
            }
        }

        return null;
    }

    public CardActionBase GetActionBase(CardName name)
    {
        for (int i = 0; i < actionList.Count; i++)
        {
            if (actionList[i].name == name)
            {
                return actionList[i];
            }
        }

        return null;
    }

}
