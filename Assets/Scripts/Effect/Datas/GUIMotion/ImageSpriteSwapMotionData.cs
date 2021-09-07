using UnityEngine.UI;
using UnityEngine;

namespace Effects
{
    [CreateAssetMenu]
    //スプライトを入れ替え、必要なら音を流す。
    public class ImageSpriteSwapMotionData : GUIMotionData
    {
        //いろいろあっていい。
        [SerializeField] GUIEffectName _name;
        public override GUIEffectName name{get{return _name;}}
        [SerializeField]protected SpriteSwapMotion _cloneBase;
        protected override ObjectEffect cloneBase{get{return _cloneBase;}}

        [System.Serializable]
        protected class SpriteSwapMotion:ObjectEffect
        {
            [SerializeField] Sprite sprite;
            [SerializeField] AudioClip sound;
            Image targetImage;
            
            public override bool compleated{get;protected set;}

            public SpriteSwapMotion():base(false)
            {}

            public override void SetTarget(MonoBehaviour target)
            {
                base.SetTarget(target);
                targetImage = target.GetComponent<Image>();

                if(targetImage == null)
                {
                    Debug.LogWarning(target.name + "don't have image");
                }
            }

            public override void Execute(RendererGetter rendererGetter, AudioSource audioSource)
            {
                if(targetImage == null)
                {
                    return;
                }

                targetImage.sprite = sprite;
                audioSource.PlayOneShot(sound);
            }
        } 
    }
}