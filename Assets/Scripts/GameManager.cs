using Utility;
using Effects;
using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(RendererGetter), typeof(AudioSource))]
public class GameManager : Singleton<GameManager>,IInteraptHandler
{
    public event Action<GameState> onGameEvent;
    //VisualEffectQueue GUIQueue;
    List<IInteraptor> interaptorQueue = new List<IInteraptor>();
    bool inputIsActive = false;
    bool guiIsActive = false;
    RendererGetter rgetter;
    AudioSource AudioSource;



    void Start()
    {
        Application.targetFrameRate = 60;
        rgetter = GetComponentInChildren<RendererGetter>();
        AudioSource = GetComponentInChildren<AudioSource>();
        StartCoroutine(GameLoop());
    }

    //今はInitialize終わったらバトル始めるだけの野蛮人。
    System.Collections.IEnumerator GameLoop()
    {
        //OnInitialize受付期間
        yield return 2;

        var gameStates = Enum.GetValues(typeof(GameState)) as GameState[];
        for (int i = 0; i < gameStates.Length; i++)
        {
            var now = gameStates[i];
            if (now == GameState.startGame)
            {
                break;
            }

            onGameEvent(gameStates[i]);
            yield return StartCoroutine(HandleInteraptors());
        }

        guiIsActive = true;
        inputIsActive = true;

        onGameEvent(GameState.startGame);

        yield return StartCoroutine(HandleInteraptors());

        onGameEvent(GameState.inBattle);
    }

    public void RegisterInterapt(IInteraptor interaptor)
    {
        interaptorQueue.Add(interaptor);
    }



    System.Collections.IEnumerator HandleInteraptors()
    {
        while (interaptorQueue.Count != 0)
        {
            guiIsActive = false;
            inputIsActive = false;
            yield return new WaitUntil(() => interaptorQueue[0].finished);
            interaptorQueue.RemoveAt(0);
        }
        guiIsActive = true;
        inputIsActive = true;
    }

    //いったんGUIQueue廃止。なぜならInstantで実行することが目的のGUIは
    //まとまった処理をまとまった単位で行うのが目的のVisualQueueと相性が良くないため。
    public void RegisterGUIMotion(IVisualEffect effect)
    {
        //GUIQueue.RegisterFX(effect);
        if(guiIsActive)
        {
            effect.Execute(rgetter,AudioSource);
        }
    }

    void Update()
    {
        if (inputIsActive)
        {
            ButtonActionQueue.instance.Trigger();
        }
    }
}

