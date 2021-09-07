using UnityEngine;
using System.Collections.Generic;
using Actor;
using System;
using Utility;

//BattleManagerベースでのEffectの更新と
//集積による情報へのアクセスポイントになる
public class PassiveHolder : Singleton<PassiveHolder>
{
    Dictionary<Character, List<PassiveEffect>> effectDataDict = new  Dictionary<Character, List<PassiveEffect>>();
    List<Character> effectedCharacter = new List<Character>();

    public event Action<Character,PassiveEffect> OnAppendEffect;

    public PassiveHolder()
    {
        BattleManager.instance.OnTurnEvent += (x) => OnTurnEvent(x);
    }


    public void AppendEffect(Character target, PassiveName name)
    {
        if (effectDataDict[target] == null)
        {
            target.Status.OnDeath += (x) => Disregister(x);
            effectDataDict[target] = new List<PassiveEffect>();
            effectedCharacter.Add(target);
        }

        var effect = PassiveEffectServer.instance.GetEffect(name);

        effectDataDict[target].Add(effect);

        OnAppendEffect(target,effect);
    }

    public void OnTurnEvent(TurnState state)
    {
        for (int i = 0; i < effectedCharacter.Count; i++)
        {
            var target = effectedCharacter[i];
            var list = effectDataDict[target];

            for (int n = 0; n < list.Count; n++)
            {
                list[i].OnTurnEvent(state, target);
            }
        }
    }

    //Targetから対象のエフェクトを取り外す。
    //帰り値は取り外せたかどうか
    //複数ある場合でも一つだけ取り除かれる。
    public bool RemoveEffect(Character target, PassiveName name)
    {
        var list = effectDataDict[target];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].name == name)
            {
                list[i].Disable(target);
                list.RemoveAt(i);

                return true;
            }
        }

        return false;
    }

    //Type = typeのすべてのパッシブを
    //targetから取り除く
    public bool RemoveEffects(Character target, PassiveType type)
    {
        var removeFlag = false;

        var list = effectDataDict[target];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].type == type)
            {
                list[i].Disable(target);
                list.RemoveAt(i);
                removeFlag = true;
            }
        }

        return removeFlag;
    }

    //呪縛からとく
    void Disregister(Character target)
    {
        var effects = effectDataDict[target];

        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Disable(target);
        }

        effectDataDict.Remove(target);
        effectedCharacter.Remove(target);
    }
}