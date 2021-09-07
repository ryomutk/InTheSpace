using System;
using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(menuName = "ObjectEffectData/TimeBarMotion")]
    public class TimeBarMotionData : CharacterStatusBarMotionData
    {
        public override ObjEffectName name { get { return ObjEffectName.timeBarMotion; } }
        [SerializeField] TimeBarMotion _timeBarMotion;
        protected override ObjectEffect cloneBase { get { return _timeBarMotion; } }

        [System.Serializable]
        protected class TimeBarMotion : CharacterStatusBarMotion
        {
            protected override void SetTargetNumNormalized()
            {
                var nowSession = BattleManager.instance.nowSession;

                if (nowSession is NowhereBattleSession nbs)
                {
                    targetNumNormalized = () => 
                    {
                        var remain = nbs.GetRemainTimeNormalized(targetCharacter);
                        return remain;
                    };
                }
                else
                {
                    Debug.LogError("I am only for Nowhere battle session");
                }
            }
        }
    }
}