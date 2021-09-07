using UnityEngine;
using System.Collections.Generic;
using Effects;
using DG.Tweening;

//決まった幅で均等に並べるLayoutField
public abstract class BasicLayoutField<T> : LayoutField<T>
where T : MonoBehaviour
{
    [SerializeField] Transform centerPoint;
    List<Transform> childList = new List<Transform>();
    [SerializeField] float overrapAmount = 0.7f;
    Vector2 objSize;

    protected void SetSize(Vector2 size)
    {
        objSize = size;
    }


    public override bool Place(T obj)
    {
        var amount = objSize.x * overrapAmount / 2;
        obj.transform.position = centerPoint.position + Vector3.right * amount * childList.Count;

        childList.Add(obj.transform);
        return true;
    }

    //ハイパー強引。でもこれサンプルだから！！サンプルだから！！！
    public override bool Remove(T obj)
    {
        int index = childList.IndexOf(obj.transform);

        childList.Remove(obj.transform);

        for (int i = index; i < childList.Count; i++)
        {
            var pos = childList[i].position;
            pos.x  -= StepAmount();

            //サンプルだから！＠！！！
            var childTrans = childList[i];
            var command = new CardSystem.CommandEffect(()=>childTrans.position = pos);
            BattleManager.instance.RegisterFX(command);
        }
        
        return true;
    }

    float StepAmount()
    {
        var amount = objSize.x * overrapAmount / 2;

        return amount; 
    }
}
