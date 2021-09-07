using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Effects;
using System;

public class VisualEffectQueue : IInteraptor
{
    public bool finished { get; private set; }
    List<IVisualEffect> queue = new List<IVisualEffect>();
    RendererGetter getterMan;
    AudioSource audioSource;
    Func<IEnumerator, Coroutine> StartCoroutine;

    public VisualEffectQueue(RendererGetter getter, AudioSource source, Func<IEnumerator, Coroutine> startCoroutine)
    {
        getterMan = getter;
        audioSource = source;
        StartCoroutine = startCoroutine;
    }

    public void RegisterFX(IVisualEffect effect)
    {
        queue.Add(effect);
    }

    public int Trigger()
    {
        var count = queue.Count;

        if (count == 0)
        {
            return 0;
        }

        StartCoroutine(HandleInput());

        return count;
    }

    IEnumerator HandleInput()
    {
        finished = false;

        BattleManager.instance.RegisterInterapt(this);

        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i].dontDisturb)
            {
                var effect = queue[i];
                effect.Execute(getterMan, audioSource);
                yield return new WaitUntil(() => effect.compleated);
            }
            else
            {
                var effect = queue[i];
                effect.Execute(getterMan, audioSource);
            }
        }

        yield return StartCoroutine(WaitForAllEffectCompleate());
        finished = true;
    }

    IEnumerator WaitForAllEffectCompleate()
    {
        while (queue.Count != 0)
        {
            yield return new WaitUntil(() => queue[0].compleated);
            queue.RemoveAt(0);
        }
    }

}