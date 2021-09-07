using DG.Tweening;
using UnityEngine;


namespace Effects
{
    /// <summary>
    /// 小さな今後変更することもないようなTweenQueueに送りたいときに使う
    /// </summary>
    public class MinimumTweenEffect : IVisualEffect
    {
        public bool compleated { get; private set; }
        public bool dontDisturb { get; private set; }
        Tween task;
        public MinimumTweenEffect(Tween tw, bool dontDisturb = false)
        {
            this.task = tw;
            compleated = false;
            this.dontDisturb = dontDisturb;
            task.onComplete += () => compleated = true;
        }

        public void Execute(RendererGetter rendererGetter, AudioSource source)
        {
            task.Play();
        }
    }
}
