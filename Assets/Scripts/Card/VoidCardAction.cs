using Actor;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu]
    public class VoidCardAction:CardActionBase
    {
        public override bool Playable{get{return true;}}
        public override CardName name{get {return CardName.card_of_void;}}
        public override string GetDetail(Actor.Character master)
        {
            return "俺は虚空教団員だぞ！！";
        }

        public override void Execute(Character master)
        {   
            var target = GetSelectedCharacter(master);
            Attack(master,100,target);
        }
    }
}