using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Effects
{
    [CreateAssetMenu(menuName = "GUIMotionData/BasicStatusBarMotionData")]
    public class BasicStatusBarMotionData : CharacterStatusBarMotionData
    {
        public override ObjEffectName name { get { return _name; } }
        [SerializeField] ObjEffectName _name;
        [SerializeField] HpBarMotion _hpBarMotion;
        protected override ObjectEffect cloneBase { get { return _hpBarMotion; } }

        [System.Serializable]
        protected class HpBarMotion : CharacterStatusBarMotion
        {
            [SerializeField] Color lowHpColor = Color.red;
            [SerializeField] Actor.CharacterStates targetState;
            Color colorDifference;

            protected override Func<float> targetNumNormalized { get; set; }
            public override void SetTarget(MonoBehaviour target)
            {
                base.SetTarget(target);
                colorDifference = lowHpColor - defaultColor;
            }

            protected override void SetTargetNumNormalized()
            {
                var defaultAmount = CharacterServer.instance.GetCharacterDefault(targetCharacter.name, targetState);
                targetNumNormalized = () =>
                {
                    var nowAmount = targetCharacter.Status.statusDictionary[targetState];
                    return nowAmount/defaultAmount;
                };
            }

            public override void Execute(RendererGetter rendererGetter, AudioSource audioSource)
            {
                base.Execute(rendererGetter, audioSource);
                targetRenderer.color = defaultColor + colorDifference * targetNumNormalized();
            }
        }


    }
}