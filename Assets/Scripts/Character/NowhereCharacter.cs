using UnityEngine;
using Effects;
using System.Collections.Generic;

namespace Actor
{
    public class CharacterInSpace : Character
    {
        [SerializeField] ObjEffectName characterMoveMotionName;
        ObjectEffect moveEffect;

        protected override void Start()
        {
            base.Start();
            Initialize();
        }

        void Initialize()
        {
            GameManager.instance.onGameEvent += (x) =>
            {
                if (x == GameState.systemInitialize)
                {
                    moveEffect = EffectServer.instance.GetObjEffect(characterMoveMotionName, this) as ObjectEffect;
                }
            };

        }

        //動けという。
        void Move()
        {
            BattleManager.instance.RegisterFX(moveEffect);
        }


    }
}