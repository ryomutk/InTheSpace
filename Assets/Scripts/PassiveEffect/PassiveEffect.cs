using UnityEngine;
using Actor;

public abstract class PassiveEffect
{
    public abstract PassiveType type{get;}
    public abstract void OnTurnEvent(TurnState state,Character target);
    //何か変化させている要素があるなら戻す。
    //取り除かれるときに呼ばれる。
    public abstract void Disable(Character target);

    public PassiveEffect Clone()
    {
        return this.MemberwiseClone() as PassiveEffect;
    }
}
