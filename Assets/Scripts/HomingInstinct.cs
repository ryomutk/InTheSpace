using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//対象のコンポーネントがDisableになるとき、もともとのヒエラルキーに戻ることを保証する
//戻ったらそのことを伝えるイベントを発生。
public class HomingInstinct : MonoBehaviour
{
    public event System.Action onDisable;
    GameObject target;
    Transform home;

    public void ChangeParent(Transform destination,GameObject target)
    {
        this.target = target;
        this.home = target.transform.parent;
        target.transform.SetParent(destination);
    }

    public void GoHome()
    {
        target.transform.SetParent(home);
    }

    void OnDisable()
    {
        if(onDisable != null)
        {
            onDisable();
        }
        Destroy(this);        
    }    
}
