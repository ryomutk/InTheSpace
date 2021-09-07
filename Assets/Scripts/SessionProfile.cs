using Actor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu]
public class SessionProfile:ScriptableObject
{
    [System.Serializable]
    struct CharaAndRate{
        [SerializeField] public CharacterName name;
        [SerializeField,Range(0,1)] public float rate;
    }

    [SerializeField] CharaAndRate[] encountData;

    public virtual CharacterName[] GetInitialCharactersInBattle()
    {
        List<CharacterName> inBattle = new List<CharacterName>();
        var dice = Random.Range(0f,1);
        
        for(int i = 0;i < encountData.Length;i++)
        {
            if(encountData[i].rate > dice)
            {
                inBattle.Add(encountData[i].name);
            }
        }

        if(inBattle.Count == 0)
        {
            inBattle.Add(encountData[0].name);
        }

        return inBattle.ToArray();
    }
}