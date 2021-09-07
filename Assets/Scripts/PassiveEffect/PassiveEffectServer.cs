using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Utility;
using System.Collections;

public class PassiveEffectServer : Singleton<PassiveEffectServer>, IInteraptor
{
    //PassiveEffectがScriptableでいいならCardActionもこれのほうが良いのでは？
    PassiveEffectData[] effects;
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

        var task = Task.Run(
            () =>
            {
                effects = Resources.LoadAll<PassiveEffectData>("Scriptables/PassiveEffect");
            }
        );

        yield return new WaitUntil(() => task.IsCompleted);

        finished = true;
    }

    public PassiveEffect GetEffect(PassiveName name)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name)
            {
                return effects[i];
            }
        }

        return null;
    }
}