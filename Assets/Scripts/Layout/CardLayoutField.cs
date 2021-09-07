using Actor;

public class CardLayoutField:BasicLayoutField<CardSystem.Card>
{
    public override bool Place(CardSystem.Card obj)
    {
        SetSize(obj.formatter.cardRect.size);
        return base.Place(obj);
    }
}