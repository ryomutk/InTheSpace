using UnityEngine;
using System.Collections.Generic;
using Actor;

/// <summary>
/// 敵を表示し、敵への入力の管理をするクラス。
/// </summary>
///
[RequireComponent(typeof(LayoutField<Character>))]
public class BattleField : MonoBehaviour
{
    List<Character> charasInBattle = new List<Character>();
    Character selected;
    public event System.Action<Character> OnCharacterSelected;
    LayoutField<Character> placer;
    [SerializeField] GUIEffectName selectedEffectName = GUIEffectName.card_motion_selected;
    [SerializeField] GUIEffectName disselectedEffectName = GUIEffectName.card_back_to_start;
    
    Effects.ObjectEffect selectedEff;
    Effects.ObjectEffect disselectedEff;

    void Start()
    {
        placer = GetComponent<LayoutField<Character>>();
    }

    public virtual void OnInput(Character subject)
    {
        var selMotion = EffectServer.instance.GetGUIMotion(selectedEffectName, subject);

        Effects.IVisualEffect disselMotion = new Effects.NullEffect();
        if(selected == subject)
        {
            selMotion = EffectServer.instance.GetGUIMotion(disselectedEffectName,subject);
            selected = null;
        }
        else if (selected != null)
        {
            disselMotion = EffectServer.instance.GetGUIMotion(disselectedEffectName, selected);
            selected = subject;
        }
        else
        {
            selected = subject;
        }
        // else
        // {
        //     disselMotion = new Effects.NullEffect();
        // }

        GameManager.instance.RegisterGUIMotion(selMotion);
        GameManager.instance.RegisterGUIMotion(disselMotion);

        OnCharacterSelected(subject);
    }

    public void AddCharacter(Character characterInstance)
    {
        ButtonSettor.instance.SetButton(characterInstance.gameObject, (x) =>
         {
             if (x.type == InputType.Click)
             {
                OnInput(characterInstance);
             }
         });

        charasInBattle.Add(characterInstance);
        placer.Place(characterInstance);
    }

    public bool RemoveCharacter(Character character)
    {
        placer.Remove(character);
        return charasInBattle.Remove(character);
    }

    public Character GetSelected()
    {
        if (charasInBattle.Contains(selected))
        {
            return selected;
        }
        else
        {
            return charasInBattle[0];
        }
    }


}