using UnityEngine;
using System;

namespace Effects
{
    public abstract class ObjectEffect : IVisualEffect
    {
        protected MonoBehaviour target{get;private set;}
        public bool dontDisturb{get;set;}
        public abstract bool compleated{get;protected set;}

        public ObjectEffect(bool dontDisturb)
        {
            compleated =false;
            this.dontDisturb = dontDisturb;
        }

        public virtual void SetTarget(MonoBehaviour target)
        {
            this.target = target;
        }

        public abstract void Execute(RendererGetter rendererGetter,AudioSource audioSource);

        public virtual ObjectEffect Clone()
        {
            return MemberwiseClone() as ObjectEffect;
        }
    }
}