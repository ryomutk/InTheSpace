using UnityEngine;
using UnityEditor;
using Utility;
using CardSystem;
using Actor;

//カード向けのJSONを作るためのエディタ拡張
public class CardJsonMaker : EditorWindow
{
    CardViewProfile jsonProfile;
    GUILayoutOption[] options = new[] {
            GUILayout.Width (64),
            GUILayout.Height (64)};

    [MenuItem("Window/CardSystem/CardJsonMaker")]
    static void Create()

    {
        GetWindow<CardJsonMaker>("CardJsonMaker");
    }

    Sprite nowSprite;

    private void OnGUI()
    {
        if (jsonProfile == null)
        {
            jsonProfile = new CardViewProfile();
        }

        using (new GUILayout.VerticalScope())
        {
            using (new GUILayout.HorizontalScope())
            {
                jsonProfile.name = (CardName)EditorGUILayout.EnumPopup(jsonProfile.name);
                if (GUILayout.Button("読み込む"))
                {
                    jsonProfile = CardProfileBuilder.Get(jsonProfile.name);
                    nowSprite = SpriteConverter.DataToSprite(jsonProfile.thumbnailData);
                }
            }
            jsonProfile.statusRequirement = (CharacterStates)EditorGUILayout.EnumFlagsField(jsonProfile.statusRequirement);
            jsonProfile.attribute = (AbilityAttribute)EditorGUILayout.EnumFlagsField(jsonProfile.attribute);
            jsonProfile.defaultWeight = EditorGUILayout.IntField("DefaultWeight", jsonProfile.defaultWeight);
            var sprite = (Sprite)EditorGUILayout.ObjectField(nowSprite, typeof(Sprite), false, options);
            if(nowSprite != sprite)
            {
                nowSprite = sprite;
                jsonProfile.thumbnailData = SpriteConverter.SpriteToData(sprite); 
                
            }
            jsonProfile.flavorText = EditorGUILayout.TextArea(jsonProfile.flavorText);
            jsonProfile.summary = EditorGUILayout.TextArea(jsonProfile.summary);

            jsonProfile.enterMotionID = (ObjEffectName)EditorGUILayout.EnumPopup("EnterMotion",jsonProfile.enterMotionID);
            jsonProfile.exitMotionID = (ObjEffectName)EditorGUILayout.EnumPopup("ExitMotion",jsonProfile.exitMotionID);
            jsonProfile.useMotionID = (ObjEffectName)EditorGUILayout.EnumPopup("UseMotion",jsonProfile.useMotionID);
            jsonProfile.selectedMotionID = (GUIEffectName)EditorGUILayout.EnumPopup("SelectMotion",jsonProfile.selectedMotionID);
            jsonProfile.disSelectedMotionID = (GUIEffectName)EditorGUILayout.EnumPopup("DisSelectMotion",jsonProfile.disSelectedMotionID);
            if (GUILayout.Button("生成"))
            {
                Write();
            }
        }
    }

    void Write()
    {
        var data = JsonHelper.GetData<CardViewProfile>(jsonProfile.name.ToString());

        if (data == null)
        {
            JsonHelper.SaveData<CardViewProfile>(jsonProfile, jsonProfile.name.ToString());
            return;
        }

        //オーバーライト実装ずみ
        JsonHelper.SaveData<CardViewProfile>(jsonProfile, jsonProfile.name.ToString());

    }

}