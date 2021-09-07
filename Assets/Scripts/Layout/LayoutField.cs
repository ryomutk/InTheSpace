using UnityEngine;

//GOを渡すとレイアウトしてくれるマン
[DisallowMultipleComponent]
public abstract class LayoutField<T>:MonoBehaviour
where T:MonoBehaviour
{
    public abstract bool Place(T obj);
    public abstract bool Remove(T obj);
}