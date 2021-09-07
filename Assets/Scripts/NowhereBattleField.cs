using UnityEngine;
using Actor;

public class NowhereBattleField:BattleField
{
    //一歩分はみんな同じで周期が違う感じにする予定。
    [SerializeField] float moveSize = 1;

        //キャラがどんだけ動くのかって話
        public float GetStepSize(Character target)
        {
            //ここでプレイヤーとぶつかってないかの判定とかもするー

            //今はとりま規定値返す
            return moveSize;
        }

}