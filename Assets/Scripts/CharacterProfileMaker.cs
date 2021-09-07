using UnityEngine;
using UnityEditor;
using Utility;
using CardSystem;
using Actor;
using System.Collections.Generic;
using System.Linq;

//カード向けのJSONを作るためのエディタ拡張
public class CharacterProfileMaker : EditorWindow
{
    CharacterProfile jsonProfile;
    GUILayoutOption[] options = new[] {
            GUILayout.Width (64),
            GUILayout.Height (64)};

    [MenuItem("Window/CardSystem/CharacterMaker")]
    static void Create()

    {
        GetWindow<CharacterProfileMaker>("CharacterMaker");
    }

    static CharacterStates[] states;
    int CardNum = 0;
    bool showCards;
    List<CardName> cardList = new List<CardName>();
    Sprite nowSprite;

    private void OnGUI()
    {
        if (jsonProfile == null)
        {
            jsonProfile = new CharacterProfile();
        }

        if (states == null)
        {
            states = System.Enum.GetValues(typeof(CharacterStates)) as CharacterStates[];
        }

        using (new GUILayout.VerticalScope())
        {
            using (new GUILayout.HorizontalScope())
            {
                jsonProfile.name = (CharacterName)EditorGUILayout.EnumPopup(jsonProfile.name);
                if (GUILayout.Button("読み込む"))
                {
                    jsonProfile = CharacterProfileBuilder.Get(jsonProfile.name);
                    nowSprite = SpriteConverter.DataToSprite(jsonProfile.texData);
                    cardList = jsonProfile.cards.ToList();
                    CardNum = cardList.Count();
                }
            }

            var sprite = (Sprite)EditorGUILayout.ObjectField(nowSprite, typeof(Sprite), false, options);
            if (sprite != nowSprite)
            {
                nowSprite = sprite;
                jsonProfile.texData = SpriteConverter.SpriteToData(sprite);
            }


            for (int i = 0; i < states.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                var state = jsonProfile.statesDataList[i].state.ToString();
                jsonProfile.statesDataList[i].amount = EditorGUILayout.IntField(state, jsonProfile.statesDataList[i].amount);
            }

            //多分これが一番簡単だと思います。
            showCards = EditorGUILayout.Foldout(showCards, "Cards");
            if (showCards)
            {
                ShowCards();
            }

            jsonProfile.enterMotionID = (ObjEffectName)EditorGUILayout.EnumPopup("EnterMotion", jsonProfile.enterMotionID);
            jsonProfile.exitMotionID = (ObjEffectName)EditorGUILayout.EnumPopup("ExitMotion", jsonProfile.exitMotionID);
            jsonProfile.deathMotionID = (ObjEffectName)EditorGUILayout.EnumPopup("DeathMotion", jsonProfile.deathMotionID);

            if (GUILayout.Button("生成"))
            {
                Write();
            }
        }
    }


    void ShowCards()
    {
        EditorGUI.indentLevel++;
        CardNum = EditorGUILayout.IntField("length", CardNum);

        for (int i = 0; i < CardNum; i++)
        {
            //タリナケレバフヤス
            if (cardList.Count == i)
            {
                if (i == 0)
                {
                    //まだなんもなければ虚空を渡す
                    cardList.Add(CardName.card_of_void);
                }
                else
                {
                    //なんかあればひとつ前のやつを入れてあげる新設設計
                    cardList.Add(cardList[i - 1]);
                }
            }

            cardList[i] = (CardName)EditorGUILayout.EnumPopup("card" + i, cardList[i]);
        }

        EditorGUI.indentLevel--;
    }

    void Write()
    {
        //カードの情報を忘れず保存。
        jsonProfile.cards = cardList.GetRange(0, CardNum).ToArray();

        var data = JsonHelper.GetData<CharacterProfile>(jsonProfile.name.ToString());

        if (data == null)
        {
            JsonHelper.SaveData<CharacterProfile>(jsonProfile, jsonProfile.name.ToString());
            return;
        }

        //オーバーライト未実装
        JsonHelper.SaveData<CharacterProfile>(jsonProfile, jsonProfile.name.ToString());

    }

}