using Actor;

public class EnergyConsumeEffectData : PassiveEffectData
{
    public override PassiveName name{get{return PassiveName.energyConsume;}}
    

    protected class EnergyConsumeEffect : PassiveEffect
    {
        public int amount { get; private set; }
        public override PassiveType type { get { return PassiveType.system; } }



        //基本的に統一したいのでとりあえず変更はできなくしておく
        TurnState updateOn = TurnState.turnStart;
        public EnergyConsumeEffect(int amount)
        {
            SetAmount(amount);
        }

        public override void OnTurnEvent(TurnState state, Character target)
        {
            if (state == updateOn)
            {
                target.Status.Modify(CharacterStates.energy, -amount);
            }
        }

        public override void Disable(Character target)
        {
            //特になし。
        }

        //新しいのつけなおすんでもいいかも。型覚えとかなきゃ出し。
        //いやそれは面倒か
        public bool SetAmount(int amount)
        {
            this.amount = amount;

            return true;
        }


    }
}
