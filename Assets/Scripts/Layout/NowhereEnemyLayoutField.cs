using UnityEngine;
using Actor;
using System.Collections.Generic;

//Basicよりも単純。決まった場所に出すだけ。勝手に歩いてくれるのでね…
public class NowhereEnemyLayoutField:LayoutField<Character>
{
    [SerializeField] Transform respawnPosition;
    List<Transform> childList = new List<Transform>();
    //並べる人の間のま
    [SerializeField] Vector2 gap = new Vector2(0,0.5f);
    //縦に並べる人の限界
    [SerializeField] int verticalCount = 6;


    public override bool Place(Character chara)
    {
        var pos = respawnPosition.position;

        //割と適当だが厳密である必要もない。
        pos -= (Vector3)gap*(childList.Count%verticalCount);
        chara.transform.position = respawnPosition.position + pos; 

        return true;
    }


    public override bool Remove(Character chara)
    {
        if(childList.Contains(chara.transform))
        {
            childList.Remove(chara.transform);
            //マジでなんもやらんな…
            return true;
        }

        return false;
    }

}