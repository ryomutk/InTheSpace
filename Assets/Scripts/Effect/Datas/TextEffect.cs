using UnityEngine;

namespace Effects
{
    public abstract class TextEffect:IVisualEffect
    {
        public abstract bool compleated{get;protected set;}
        public bool dontDisturb{get;private set;}
        protected Transform target{get;private set;}
        protected string content{get;private set;}

        public TextEffect(bool dontDisturb)
        {
            compleated = false;
            this.dontDisturb = dontDisturb;
        }

        public virtual void SetContent(Transform target,string content)
        {
            this.target = target;
            this.content = content;
        }
        public abstract void Execute(RendererGetter rendererGetter,AudioSource source);

        public TextEffect Clone()
        {
            return MemberwiseClone() as TextEffect;
        }
    }
}