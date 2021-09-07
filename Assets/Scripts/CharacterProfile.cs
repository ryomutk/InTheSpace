using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Actor;


namespace Actor
{
    [Serializable]
    public class CharacterProfile
    {

        [SerializeField] CardSystem.CardName[] _cards;

        //誰にでも共通してそんざいしているモーション
        [SerializeField] ObjEffectName _enterMotionID;
        [SerializeField] ObjEffectName _exitMotionID;
        [SerializeField] ObjEffectName _deathMotionID;
        [SerializeField] TextureData _texData;
        [SerializeField] CharacterName _name;
        [SerializeField] List<CharacterStatesData> _statesList = new List<CharacterStatesData>();

        public Sprite defaultThumbnail { get; private set; }
        public TextureData texData
        {
            get { return _texData; }
            set
            {
                _texData = value;
                //これちょっとやばいかな？
                defaultThumbnail = SpriteConverter.DataToSprite(texData);
            }
        }

        public List<CharacterStatesData> statesDataList { get { return _statesList; } }
        public CharacterName name { get { return _name; } set { _name = value; } }
        public CardSystem.CardName[] cards { get { return _cards; } set { _cards = value; } }
        public ObjEffectName enterMotionID { get { return _enterMotionID; } set { _enterMotionID = value; } }
        public ObjEffectName exitMotionID { get { return _exitMotionID; } set { _exitMotionID = value; } }
        public ObjEffectName deathMotionID { get { return _deathMotionID; } set { _deathMotionID = value; } }

        public CharacterProfile()
        {
            CharacterStates[] states = System.Enum.GetValues(typeof(CharacterStates)) as CharacterStates[];
            for (int i = 0; i < states.Length; i++)
            {
                var data = new CharacterStatesData(states[i]);
                
                statesDataList.Add(data);
            }
        }


        //今後Character側の初期化手法などが変わることも考えて、その際にここで対応できるようにこちら側で
        //ロード用メソッドを用意。消して公開が面倒だったわけではない。
        //さらに、キャラのイベントにここで行動の呼び出しを書いておくことで
        //Motion何があるかをCharaが把握していなくてもよいようにする
        //ロード手法までデータに書くの、ちょっといいかもしれない。
        public void LoadToCharacter(Character targetInstance)
        {
            targetInstance.name = name;



            targetInstance.Status.Initialize();
            for(int i  = 0; i < statesDataList.Count;i++)
            {
                targetInstance.Status.statusDictionary[statesDataList[i].state] = statesDataList[i].amount;
            }

            var enter = EffectServer.instance.GetObjEffect(_enterMotionID, targetInstance);
            targetInstance.OnEnter += (target) => BattleManager.instance.RegisterFX(enter);

            var exit = EffectServer.instance.GetObjEffect(_exitMotionID, targetInstance);
            targetInstance.OnExit += (target) => BattleManager.instance.RegisterFX(exit);

            var death = EffectServer.instance.GetObjEffect(_deathMotionID, targetInstance);
            targetInstance.Status.OnDeath += (target) => BattleManager.instance.RegisterFX(death);

            SetDeck(targetInstance);


            if (targetInstance.viewRenderer != null)
            {
                if (defaultThumbnail == null)
                {
                    defaultThumbnail = SpriteConverter.DataToSprite(_texData);
                }
                targetInstance.viewRenderer.sprite = defaultThumbnail;
            }
        }



        void SetDeck(Character target)
        {
            BattleManager.instance.deckHolder.RegisterCharacter(target);
            var weightData = BattleManager.instance.deckHolder.GetWeightData(target);

            foreach (var card in cards)
            {
                weightData.AddCard(card);
            }
        }
    }
}

//Serializeするためのクラス。Dictionaryの代わり。
[Serializable]
public class CharacterStatesData
{
    [SerializeField] int _amount;
    [SerializeField] CharacterStates _state;
    [SerializeField] public int amount{get{return _amount;}set{_amount = value;}}
    [SerializeField] public CharacterStates state{get{return _state;} set{_state = value;}}

    public CharacterStatesData(CharacterStates state)
    {
        this.state = state;
    }
}