using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Gui表示だけのために使われるデータ
    /// </summary>
    public abstract class GUIMotionData : ScriptableObject
    {
        new public abstract GUIEffectName name { get; }
        protected abstract ObjectEffect cloneBase { get; }

        protected ObjectEffect InitMotion(MonoBehaviour target)
        {
            var instance = cloneBase.Clone();
            instance.SetTarget(target);

            return instance;
        }

        public virtual ObjectEffect GetMotion(MonoBehaviour target)
        {
            var instance = InitMotion(target);
            
            return instance;
        }
    }
}


