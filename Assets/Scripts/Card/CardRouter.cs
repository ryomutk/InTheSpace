using UnityEngine;
using Utility;
using System;

namespace CardSystem
{
    public class CardRouter:Singleton<CardRouter>
    {
        public event System.Action<Card,InputArg> OnCardInput;
        
        public void SetCard(ICardViewPoint viewPoint,CardName name)
        {
            var cardInstance = CardServer.instance.GetCard(name);
            
            //カードを監視したい人
            ButtonAction inputAction = (x) => OnCardInput(cardInstance,x);

            //カードにたいする効果
            inputAction += (x) => viewPoint.OnInput(cardInstance,x);
            inputAction += (x) => cardInstance.ButtonCallback(x);

            ButtonSettor.instance.SetButton(cardInstance.gameObject,inputAction);
        }


    }
}