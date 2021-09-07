using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//Turnの中で動かしたいUIの、
//アップデートタイミングを保持し、
//GameManagerに登録登録
//ちなみにTurnBaseなのでもちろんGUIではなくObjectEffect
[System.Serializable]
public class TurnBaseUI
{
    [SerializeField] List<TurnState> _updateOn;
    [SerializeField] ObjEffectName _baseEffectName;
    public bool active{get;private set;}
    List<TurnState> updateOn { get { return _updateOn; } set { updateOn = value; } }

    ObjEffectName baseEffectName { get { return _baseEffectName; } set { _baseEffectName = value; } }
    Effects.IVisualEffect baseEffect;
    System.Action<TurnState> updateCallback;

    protected virtual void UpdateCallback(TurnState state)
    {
        if (updateOn.Contains(state))
        {
            BattleManager.instance.RegisterFX(baseEffect);
        }
    }

    public void Activate()
    {
        BattleManager.instance.OnTurnEvent += updateCallback;
    }

    public void DisActivate()
    {
        BattleManager.instance.OnTurnEvent -= updateCallback;
    }

    public TurnBaseUI Clone(MonoBehaviour target)
    {
        //ここまできたなら使える子供は残っていない。

        var clone = (TurnBaseUI)MemberwiseClone();
        clone.baseEffect = EffectServer.instance.GetObjEffect(baseEffectName, target); 
        clone.updateCallback = (x) => clone.UpdateCallback(x);

        return clone;
    }
}