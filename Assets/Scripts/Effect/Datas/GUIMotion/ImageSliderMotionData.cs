using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
using Sirenix.OdinInspector;

namespace Effects
{
    //数値(Normalized)でImageの長さを変えるマン。
    //観測するための数字が必要なので継承して使おう。
    public abstract class ImageSliderMotionData : ObjectEffectData
    {

        [SerializeField]
        protected abstract class ImageSliderMotion : ObjectEffect
        {
            protected Image targetRenderer;
            [SerializeField] Sprite baseSprite;
            //こいつが基準。
            protected abstract System.Func<float> targetNumNormalized { get; set; }


            float lastScale;
            //出現時にうにょーんってするか
            [SerializeField] bool hasEnterMotion = true;
            [SerializeField] float initialScale = 1;
            //縦か？(変えるScaleがYか？と等価)
            [SerializeField] bool isVertical = false;
            [SerializeField] protected Color defaultColor = Color.black;
            [SerializeField, ShowIf("hasEnterMotion")] float enterDuration = 2;
            [SerializeField] float changeDuration;
            [SerializeField] Ease changeEase = Ease.OutQuad;
            [SerializeField] Vector2 defaultSizeDelta = new Vector2(100, 10);
            [SerializeField] bool allowMinus = false;

            /// <summary>
            /// 注意！お前は無視される。
            /// </summary>
            /// <value></value>
            public override bool compleated { get { return true; } protected set { } }
            bool entering = false;

            protected ImageSliderMotion() : base(false)
            { }

            public override void SetTarget(MonoBehaviour target)
            {
                base.SetTarget(target);
                //終わってなくても待つ必要がない。
                compleated = true;
                if (hasEnterMotion)
                {
                    entering = true;
                }
            }

            void InitBarView()
            {
                if (hasEnterMotion)
                {
                    if (isVertical)
                    {
                        targetRenderer.transform.localScale = new Vector3(1, 0, 1);
                    }
                    else
                    {
                        targetRenderer.transform.localScale = new Vector3(0, 1, 1);
                    }
                }
                else
                {
                    if (initialScale != 1)
                    {
                        if (isVertical)
                        {
                            targetRenderer.transform.localScale = new Vector3(1, initialScale, 1);
                        }
                        else
                        {
                            targetRenderer.transform.localScale = new Vector3(initialScale, 1, 1);
                        }
                    }
                }

                targetRenderer.rectTransform.sizeDelta = defaultSizeDelta;
            }

            public override void Execute(RendererGetter rendererGetter, AudioSource audioSource)
            {

                if (entering)
                {
                    entering = false;

                    if (targetRenderer == null)
                    {
                        targetRenderer = rendererGetter.GetImageObj();
                        InitBarView();
                        targetRenderer.sprite = baseSprite;
                        targetRenderer.color = defaultColor;
                    }

                    //見た目が変わる前に次のが呼ばれる可能性があるので、それにも対応できるように概念上では先に代入しておく。
                    lastScale = initialScale;

                    Tween tw;
                    //これは直前にSetTargetされ、なおかつEnterMotionがあるときと等価。
                    if (isVertical)
                    {
                        tw = targetRenderer.rectTransform.DOScaleY(initialScale, enterDuration);
                    }
                    else
                    {
                        tw = targetRenderer.rectTransform.DOScaleX(initialScale, enterDuration);
                    }

                    tw.Play();
                }
                else
                {
                    if (targetRenderer == null)
                    {
                        //Entering出ないときにTargetRenderer==null => 利用中止状態ということ
                        return;
                    }

                    var targetNum = targetNumNormalized();
                    if (targetNum < 0 && !allowMinus)
                    {
                        targetNum = 0;
                    }

                    //変化していたら
                    if (lastScale != targetNum)
                    {
                        Tween tw;
                        //更新
                        if (isVertical)
                        {
                            tw = targetRenderer.rectTransform.DOScaleY(targetNum, changeDuration);
                        }
                        else
                        {
                            tw = targetRenderer.rectTransform.DOScaleX(targetNum, changeDuration);
                        }

                        tw.SetEase(changeEase);
                        tw.Play();

                        lastScale = targetNum;
                    }
                }
            }

            /// <summary>
            /// 使い終わったら必ず呼ぶこと！
            /// </summary>
            protected void ReleaseResources()
            {
                targetRenderer.gameObject.SetActive(false);
                targetRenderer = null;
            }
        }
    }
}