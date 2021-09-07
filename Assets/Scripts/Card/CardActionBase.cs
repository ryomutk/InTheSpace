using UnityEngine;
using Actor;
using System.Collections.Generic;
using Effects;
using Utility;


namespace CardSystem
{
    public abstract class CardActionBase : ScriptableObject
    {
        new public abstract CardName name { get; }
        //使用できるか
        public abstract bool Playable{get;}
        public abstract string GetDetail(Character master);
        public abstract void Execute(Character master);

        TextEffectName damageEfxName = TextEffectName.damage_text_effect;
        TextEffectName missEfxName = TextEffectName.miss_text_effect;


        //再利用する。捨てるのもったいないから。
        //お行儀は結構悪い。
        static List<TextEffect> damageEffects = new List<TextEffect>();
        static List<TextEffect> missEffects = new List<TextEffect>();

        protected Character GetSelectedCharacter(Character master)
        {
            return BattleManager.instance.nowSession.GetSelected();
        }

        protected Character GetSelectedEnemy(Character master)
        {
            if (PlayerManager.instance.IsPlayer(master))
            {
                return GetSelectedCharacter(master);
            }
            else
            {
                return PlayerManager.instance.GetPlayer();
            }
        }

        //誰かを攻撃。デフォルトはnullで、今選択されている相手を攻撃。
        protected bool Attack(Character master, int damage, Character target = null)
        {
            Character to;
            if (target == null)
            {
                to = GetSelectedEnemy(master);
            }
            else
            {
                to = target;
            }


            IVisualEffect efx;


            efx = GetDamageEffect(to, damage.ToString());
            BattleManager.instance.RegisterFX(efx);
            System.Action action = () => to.Status.Modify(CharacterStates.hp, -damage);
            var command = new CommandEffect(action);
            BattleManager.instance.RegisterFX(command);

            return true;

            /*
                        catch (System.Exception)
                        {
                            efx = GetMissEffect(master);
                            BattleManager.instance.RegisterFX(efx);
                        }
            */

            //return false;
        }


        TextEffect GetDamageEffect(Character target, string content)
        {
            for (int i = 0; i < damageEffects.Count; i++)
            {
                if (damageEffects[i].compleated)
                {
                    damageEffects[i].SetContent(target.transform, content);
                }
            }

            var efx = EffectServer.instance.GetTextEffect(damageEfxName, target.transform, content) as TextEffect;
            damageEffects.Add(efx);
            return efx;
        }

        TextEffect GetMissEffect(Character target)
        {
            for (int i = 0; i < missEffects.Count; i++)
            {
                if (missEffects[i].compleated)
                {
                    missEffects[i].SetContent(target.transform, "MISS!!!");
                }
            }

            var efx = EffectServer.instance.GetTextEffect(missEfxName, target.transform, "MISS!") as TextEffect;
            damageEffects.Add(efx);
            return efx;
        }



        ~CardActionBase()
        {
            damageEffects = null;
            missEffects = null;
        }
    }

    //関数呼び出しをQueueの中でできるチートアイテム。
    //お母さんには内緒。
    public class CommandEffect : IVisualEffect
    {
        public bool compleated { get { return true; } }
        public bool dontDisturb { get { return false; } }
        System.Action action;

        public CommandEffect(System.Action command)
        {
            this.action = command;
        }

        public void Execute(RendererGetter getter, AudioSource source)
        {
            action();
        }
    }
}