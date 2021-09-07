using UnityEngine;

namespace Effects

{
    public abstract class TextEffectData :ScriptableObject
    {
        new public abstract TextEffectName name{get;}
        protected abstract TextEffect cloneBase{get;}

        protected TextEffect InitMotion(Transform target,string content)
        {
            var instance = cloneBase.Clone();
            instance.SetContent(target,content);

            return instance;
        }
        public virtual IVisualEffect GetMotion(Transform target,string content)
        {
            var instance = InitMotion(target,content);
            return instance;
        }
    }
}