using UnityEngine;
using DG.Tweening;
using System;

namespace Effects
{
    [CreateAssetMenu]
    public class CardUsedMotionData : ObjectEffectData
    {
        public override ObjEffectName name{get{return ObjEffectName.card_motion_use;}}
        [SerializeField] CardUsedMotion _cloneBase;
        protected override ObjectEffect cloneBase{get{return _cloneBase;}}



        [System.Serializable]
        class CardUsedMotion : ObjectEffect
        {
            [SerializeField] float duration = 0.7f;
            [SerializeField] AudioClip usedSound;
            [SerializeField] Ease ease = Ease.InOutBounce;
            Tween tween;
            UnityEngine.UI.Image cardImage;
            public override bool compleated { get; protected set;}

            public CardUsedMotion():base(true){}

            public override void SetTarget(MonoBehaviour target)
            {

                base.SetTarget(target);
                var sq = DOTween.Sequence();
                sq.onComplete += () => compleated = true;

                cardImage = target.GetComponentInChildren<UnityEngine.UI.Image>();

                if (cardImage != null)
                {
                    sq.Append(cardImage.DOColor(Color.white, duration / 3))
                    .Join(target.transform.DOScaleY(0, duration*2/3).SetEase(ease))
                    .Join(cardImage.DOFade(0,duration));
                }

                tween = sq;
            }

            public override void Execute(RendererGetter rendererGetter, AudioSource audioSource)
            {
                audioSource.PlayOneShot(usedSound);
                tween.Play();
            }

        }
    }
}