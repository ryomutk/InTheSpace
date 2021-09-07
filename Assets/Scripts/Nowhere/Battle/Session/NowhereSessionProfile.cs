using Actor;
using UnityEngine;

[CreateAssetMenu(menuName = "SessionProfile/NowhereSessionProfile")]

public class NowhereSessionProfile : SessionProfile, ITimerObject
{
    [SerializeField] int _dexterity;
    public int dexterity { get { return _dexterity; } }

    public override CharacterName[] GetInitialCharactersInBattle()
    {
        BattleManager.instance.nowSession.RegisterTimerObject(this);
        return base.GetInitialCharactersInBattle();
    }

    public void NoticeTime()
    {
        var additional = base.GetInitialCharactersInBattle();

        for (int i = 0; i < additional.Length; i++)
        {
            BattleManager.instance.nowSession.AddCharacter(additional[i]);
        }
    }
}