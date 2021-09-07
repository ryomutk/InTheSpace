using UnityEngine;
using System.Collections.Generic;

namespace Effects
{
    //Spriteを変える上に前に回り込ませもする
    [CreateAssetMenu(menuName = "GUIMotionData/SpriteAppearMotion")]
    public class SpriteAppearMotionData :ImageSpriteSwapMotionData
    {
        public SpriteAppearMotionData()
        {
            _cloneBase = new SpriteAppearMotion();
        }

        protected class SpriteAppearMotion:SpriteSwapMotion
        {
            public override void Execute(RendererGetter rendererGetter, AudioSource audioSource)
            {
                base.Execute(rendererGetter, audioSource);
                target.transform.SetSiblingIndex(0);
            }
        }
    }
}