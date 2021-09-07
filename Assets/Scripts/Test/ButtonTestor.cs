using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Image))]
public class ButtonTestor : MonoBehaviour
{
    Image image;

    [Button]
    void Register()
    {
        ButtonSettor.instance.SetButton(gameObject,(arg) => ButtonAction(arg));
    }



    void ButtonAction(InputArg arg)
    {
        if(arg.type == InputType.Click)
        {
            Debug.Log("click");
        }
        else if(arg.type == InputType.hoverEnd)
        {
            Debug.Log("hover end");
        }
        else if(arg.type == InputType.hoverStart)
        {
            Debug.Log("hover start");
        }
        else if(arg.type == InputType.pointerDown)
        {
            Debug.Log("pointer down");
        }
        else if(arg.type == InputType.pointerUp)
        {
            Debug.Log("pointer up");    
        }
    }
    
}
