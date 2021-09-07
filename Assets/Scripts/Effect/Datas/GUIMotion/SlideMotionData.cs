using UnityEngine;
using DG.Tweening;
using System;

namespace Effects
{
    [CreateAssetMenu]
    public class SlideMotionData :GUIMotionData
    {

        public override GUIEffectName name { get { return _name; } }
        [SerializeField] GUIEffectName _name;
        [SerializeField] SlideMotion _cloneableInstance;
        protected override ObjectEffect cloneBase{get{return _cloneableInstance;}}

        [System.Serializable]
        class SlideMotion : ObjectEffect
        {
            Tween tween;
            [SerializeField] float duration = 0.5f;
            [SerializeField] string cursorTrigger = "Show";
            [SerializeField] float slideAmount = 0.7f;
            [SerializeField] Ease ease = Ease.InOutBounce;
            [SerializeField] AudioClip sound;
            public override bool compleated{get;protected set;}

            public SlideMotion() : base(false)
            { }

            public override void SetTarget(MonoBehaviour target)
            {
                base.SetTarget(target);
                var posy = target.transform.position.y;
                tween = target.transform.DOMoveY(posy + slideAmount, duration)
                .SetEase(ease);
                tween.onComplete = () => compleated = true;
            }

            public override void Execute(RendererGetter rendererGetter, AudioSource audioSource)
            {
                if(sound != null)
                {
                    audioSource.PlayOneShot(sound);
                }

                tween.Play();
            }
        }
    }
}