using System;
using UnityEngine;
using Utility;
using System.Collections;
using System.Collections.Generic;


public class AnimationServer : Singleton<AnimationServer>
{
    [SerializeField] AnimatorOverrideController voidAnimator;
    List<AnimatorOverrideController> animators = new List<AnimatorOverrideController>();

    protected void Start()
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
        var task = new InteraptTask();
        GameManager.instance.RegisterInterapt(task);
        yield return null;

        var fileNames = System.IO.Directory.GetFiles(Application.dataPath + "/Resources/Animator/Character/", "*.overrideController");
        for (int i = 0; i < fileNames.Length; i++)
        {
            var filename = System.IO.Path.GetFileNameWithoutExtension(fileNames[i]);
            var req = Resources.LoadAsync<AnimatorOverrideController>("Animator/Character/"+filename);
            yield return new WaitUntil(()=> req.isDone);
            animators.Add(req.asset as AnimatorOverrideController);
        }

        task.finished = true;
    }

    public AnimatorOverrideController GetOverrideController(Actor.CharacterName name)
    {
        for(int i = 0;i < animators.Count;i++)
        {
            if(animators[i].name == name.ToString())
            {
                return animators[i];
            }
        }

        Debug.LogWarning("Animtor name with "+ name +"is not found");

        return voidAnimator;
    }
}