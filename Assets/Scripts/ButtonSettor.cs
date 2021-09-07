using UnityEngine;
using Utility.ObjPool;
using Utility;
using System.Collections;

public class ButtonSettor:Singleton<ButtonSettor>
{
    [SerializeField] int initNum = 10;
    [SerializeField] CustomButton buttonPref;
    InstantPool<CustomButton> buttonPool;

    void Start()
    {
        buttonPool = new InstantPool<CustomButton>(transform);
        buttonPool.CreatePool(buttonPref,initNum);
    }

    public void SetButton(GameObject target, ButtonAction eventAction)
    {   
        var instance = buttonPool.GetObj();
        
        instance.AttachSelf(target);
        instance.buttonEvent += eventAction;
    }
}