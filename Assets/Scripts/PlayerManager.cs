using UnityEngine;
using Utility;
using Actor;
using System.Collections;
using System.Collections.Generic;

//プレイヤーの初期化や情報の読み込み、ストレージを行い、
//プレイヤーの情報へのアクセスポイントや各種設定。を行う
public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] Character rawPlayerPref;
    List<Character> playerList = new List<Character>();
    Dictionary<Character,int> handNumberList = new Dictionary<Character, int>();
    public int playerNum{get{return playerList.Count;}}

    void Start()
    {
        GameManager.instance.onGameEvent += (x) =>
        {
            if (x == GameState.managerInitialize)
            {
                StartCoroutine(Initialize());
            }
        };
    }

    /// <summary>
    /// 今はプレイヤーは一人のみの想定。
    /// でも簡単に複数に増やせます。
    /// </summary>
    /// <returns></returns>
    IEnumerator Initialize()
    {
        var task = new InteraptTask();
        GameManager.instance.RegisterInterapt(task);

        var profile = CharacterServer.instance.GetProfile(CharacterName.player);
        var player = Instantiate(rawPlayerPref);
        profile.LoadToCharacter(player);

        playerList.Add(player);
        
        yield return null;

        task.finished = true;
    }

    public bool IsPlayer(Character character)
    {
        return playerList.Contains(character);
    }

    //原初のプレイやーデータへのアクセス。
    //プレイヤーが1人のみの場合は何も渡さなくても通る。
    public Character GetPlayer(int index = -1)
    {
        if(index == -1)
        {
            if(playerNum == 1)
            {
                return playerList[0];
            }
            else
            {
                //複数人拡張は(以下略)
                throw new System.NotImplementedException();
            }
        }
        return playerList[index];
    }

    void SavePlayerStatus()
    {
        
    }


}