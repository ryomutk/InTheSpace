using Actor;
using UnityEngine;

public class CharacterLayoutField:BasicLayoutField<Character>
{
    public override bool Place(Character obj)
    {
        SetSize(obj.viewRenderer.rectTransform.rect.size);
        return base.Place(obj);
    }
}