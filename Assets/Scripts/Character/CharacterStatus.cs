using System.Collections.Generic;
using UnityEngine;
using Effects;

namespace Actor

{
    //キャラがダメージを受けた時のリアクションと値の管理。
    public class CharacterStatus
    {
        public bool isAlive { get { return statusDictionary[CharacterStates.hp] >= 0; } }
        public Dictionary<CharacterStates, int> statusDictionary = new Dictionary<CharacterStates, int>();
        //しんだとき
        public event CharacterAction OnDeath;
        //被害を受けたとき
        public event System.Action<Character, CharacterStates, int> OnModify;
        protected Character master;

        public CharacterStatus(Character master)
        {
            this.master = master;
        }


        public virtual bool Modify(CharacterStates status, int ammount)
        {
            if (!isAlive)
            {
                return false;
            }

            statusDictionary[status] += ammount;

            if (OnModify != null)
            {
                OnModify(master, status, ammount);
            }

            if (status == CharacterStates.hp && statusDictionary[status] < 0)
            {
                if (OnDeath != null)
                {
                    OnDeath(master);
                }
            }

            return true;
        }

        public void Initialize()
        {
            OnDeath = null;
            OnModify = null;
        }

    }
}