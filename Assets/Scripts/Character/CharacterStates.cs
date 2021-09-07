using System;

namespace Actor
{
    //キャラのステ一覧、数的な数の基準はクトゥルフの50倍くらいが良いかなと思う
    [Flags]
    public enum CharacterStates
    {
        none,
        hp = 1,        //プレイヤー上限3000程度。敵は比較的高く。
        dexterity = 2, //どれほど器用にものを扱えるかに影響。
        energy = 4, 
        fuel = 8,      //これはロケットだけのものかもー
        capacity = 16
    }
}