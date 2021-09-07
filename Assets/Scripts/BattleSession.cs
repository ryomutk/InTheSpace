using UnityEngine;
using System.Collections.Generic;
using Actor;

//現在のバトルの状況を保持し、
//各種コンポーネントへのアクセスポイントになるActorのCardRouterみたいな？
//ターン内の動きもこれを拡張して定義。
public class BattleSession
{
    protected List<Character> characterInBattle = new List<Character>();
    protected List<Character> killedList = new List<Character>();
    protected BattleField field;
    public event System.Action<Character> OnCharacterKilled;
    public event System.Action<Character> OnCharacterSelected;
    public bool IfBattleEnd { get; private set; }

    System.Action<BattleState> initCallback;

    public BattleSession(SessionProfile profile, BattleField field)
    {
        this.field = field;
        field.OnCharacterSelected += (x) => { };

        initCallback = (x) =>
        {
            if (x == BattleState.dataInitialize)
            {
                BattleInitialize(profile);
            }
        };

        BattleManager.instance.OnBattleEvent += initCallback;

    }

    public void BattleInitialize(SessionProfile profile)
    {
        if (PlayerManager.instance.playerNum == 1)
        {

            var player = PlayerManager.instance.GetPlayer(0);
            AddCharacter(player.name);
        }
        else
        {
            //複数人拡張はまだ作ってないよ。
            throw new System.NotImplementedException();
        }

        foreach (var enemy in profile.GetInitialCharactersInBattle())
        {
            AddCharacter(enemy);
        }

        BattleManager.instance.OnBattleEvent -= initCallback;
    }

    public virtual Character AddCharacter(CharacterName name)
    {
        if (name == CharacterName.player)
        {
            if (PlayerManager.instance.playerNum != 1)
            {
                //まだ複数人拡張は()
                throw new System.NotImplementedException();
            }

            var player = PlayerManager.instance.GetPlayer(0);
            characterInBattle.Add(player);

            return player;
        }
        var instance = CharacterServer.instance.GetCharacter(name);
        field.AddCharacter(instance);
        characterInBattle.Add(instance);
        instance.Enter();

        instance.Status.OnDeath += (x) => OnKilled(x);

        return instance;
    }

    protected virtual void OnKilled(Character target)
    {
        killedList.Add(target);
        characterInBattle.Remove(target);
        field.RemoveCharacter(target);
    }

    protected virtual void CheckIfEnd()
    {
        for (int i = 0; i < characterInBattle.Count; i++)
        {
            var target = characterInBattle[i];
            var result = PlayerManager.instance.IsPlayer(target);

            if (!result)
            {
                IfBattleEnd = false;
                return;
            }
        }

        IfBattleEnd = true;
    }

    public Character GetSelected()
    {
        return field.GetSelected();
    }

}