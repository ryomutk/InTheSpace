using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Effects
{
    [CreateAssetMenu(menuName = "TextEffect/Simple")]
    //対象者の上(over)にふわっと指定した文字を指定した形式で出すエフェクト。
    public class SimpleTextEffectData : TextEffectData
    {
        [SerializeField] TextEffectName _name;
        public override TextEffectName name { get { return _name; } }
        [SerializeField] SimpleTextEffect _cloneBase = new SimpleTextEffect();
        protected override TextEffect cloneBase { get { return _cloneBase; } }

        [System.Serializable]
        class SimpleTextEffect : TextEffect
        {

            [SerializeField] TMP_FontAsset useFont;
            [SerializeField] float duration = 0.5f;
            [SerializeField] Ease ease = Ease.OutQuad;
            [SerializeField] float moveSize = 1;
            public override bool compleated { get; protected set; }

            public SimpleTextEffect() : base(true)
            { }

            public override void SetContent(Transform target, string contents)
            {
                //再装填可能
                compleated = false;

                base.SetContent(target, contents);
            }


            public override void Execute(RendererGetter rendererGetter, AudioSource source)
            {
                var sq = DOTween.Sequence();
                var text = rendererGetter.GetTextObj();
                if (text.font != useFont)
                {
                    text.font = useFont;
                }

                sq.onComplete = () => compleated = true;

                text.transform.position = target.position;
                text.color = Color.clear;
                text.text = content;


                sq.Append(text.transform.DOLocalMoveY(moveSize, duration).SetRelative())
                .Join(text.DOFade(1, duration / 3))
                .Append(text.DOFade(0, duration * 2 / 3));

                sq.SetEase(ease);

                sq.onComplete += () =>
                {
                    compleated = true;
                    text.gameObject.SetActive(false);
                };


                sq.Play();
            }
        }
    }
}