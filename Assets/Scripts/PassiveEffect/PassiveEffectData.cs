using UnityEngine;

public abstract class PassiveEffectData:ScriptableObject
{
    new public abstract PassiveName name{get;}
    protected abstract PassiveEffect cloneBase{get;}

    public virtual PassiveEffect GetInstance()
    {
        return cloneBase.Clone();
    }
}