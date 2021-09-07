using UnityEngine;
using System.Collections.Generic;
using CardSystem;

public abstract class RocketEquipment
{
    public abstract bool upgradeable{get;}
    public abstract string discription{get;}
    protected List<Card> additionalCard;
    protected List<PassiveName> passives;
}