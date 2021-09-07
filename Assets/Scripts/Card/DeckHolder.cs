using UnityEngine;
using System.Collections.Generic;
using CardSystem;
using Actor;
using System.Linq;

//キャラとそのデッキ情報
//CardNameとWeightの組を持ちし者
//BattleMangerからアクセスさせる予定
//ActorとCardの情報平面。
public class DeckHolder
{
    Dictionary<Character, DeckWeightData> deckWeightDatas = new Dictionary<Character, DeckWeightData>();

    public void RegisterCharacter(Character chara)
    {
        deckWeightDatas[chara] = new DeckWeightData();
        chara.Status.OnDeath += (x) => Disregister(x);
    }

    public DeckWeightData GetWeightData(Character chara)
    {
        return deckWeightDatas[chara];
    }

    //基本キャラが死んだら自動で呼ばれるようにする
    void Disregister(Character character)
    {
        deckWeightDatas.Remove(character);
    }
}

public class DeckWeightData
{
    public List<CardName> nameList { get; private set; }
    public List<int> weightList { get; private set; }

    public DeckWeightData()
    {
        nameList = new List<CardName>();
        weightList = new List<int>();
    }

    public void AddCard(CardName name)
    {
        var weight = CardServer.instance.GetProfile(name).defaultWeight;
        nameList.Add(name);
        weightList.Add(weight);

    }

    void AddCard(CardViewProfile card)
    {
        nameList.Add(card.name);
        weightList.Add(card.defaultWeight);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="removeAll"></param>
    /// <returns>if successed remove</returns>
    public bool RemoveCard(CardName card, bool removeAll = true)
    {
        int index = nameList.IndexOf(card);

        if (index != -1)
        {
            nameList.RemoveAt(index);
            weightList.RemoveAt(index);
            if (!removeAll)
            {
                return true;
            }
        }
        else
        {
            return false;
        }

        while (index != -1)
        {
            nameList.RemoveAt(index);
            weightList.RemoveAt(index);
        }

        return true;
    }

    public bool ModifyWeight(CardName target, int amount, bool modifyAll = true)
    {
        var result = false;

        for (int i = 0; i < nameList.Count; i++)
        {
            if (nameList[i] == target)
            {
                weightList[i] += amount;
                result = true;

                if (!modifyAll)
                {
                    return true;
                }
            }
        }

        return result;
    }


    public CardName DrawCard()
    {
        if (weightList.Count != nameList.Count)
        {
            Debug.LogError("Card and Weight not match");

            return CardName.card_of_void;
        }

        int total = 0;
        for (int i = 0; i < nameList.Count; i++)
        {
            total += weightList[i];
        }

        int target = Random.Range(0, total);

        for (int i = 0; i < nameList.Count; i++)
        {
            target -= weightList[i];
            if (target <= 0)
            {
                return nameList[i];
            }
        }

        Debug.LogError("Something went wrong");
        return CardName.card_of_void;
    }

}