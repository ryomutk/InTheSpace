using UnityEngine;
using UnityEngine.EventSystems;

public delegate void ButtonAction(InputArg arg);
public class CustomButton : UIBehaviour
{
    public event ButtonAction buttonEvent;
    HomingInstinct instinct;

    KeyCode[] observeKeys = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse3 };
    ButtonAction nullAction;


    public bool clearEventsOnDisable = true;

    protected override void Start()
    {
        base.Start();
        nullAction = (x) => { };
    }

    public void AttachSelf(GameObject target)
    {
        target.AddComponent<HomingInstinct>().ChangeParent(transform, target);
        instinct = GetComponent<HomingInstinct>();
    }


    public void OnPointerDown()
    {
        var arg = new InputArg();
        for (int i = 0; i < observeKeys.Length; i++)
        {
            if (Input.GetKeyDown(observeKeys[i]))
            {
                arg.key = observeKeys[i];
                break;
            }
        }

        arg.type = InputType.pointerDown;

        ButtonActionQueue.instance.RegisterAction(() => buttonEvent(arg));
    }

    public void OnHover()
    {
        var arg = new InputArg();

        arg.type = InputType.hoverStart;
        arg.key = KeyCode.None;
        ButtonActionQueue.instance.RegisterAction(() => buttonEvent(arg));
    }

    public void OnOffHover()
    {
        var arg = new InputArg();

        arg.type = InputType.hoverEnd;
        arg.key = KeyCode.None;
        ButtonActionQueue.instance.RegisterAction(() => buttonEvent(arg));
    }

    public void OnPointerUp()
    {
        var arg = new InputArg();

        arg.type = InputType.pointerUp;

        for (int i = 0; i < observeKeys.Length; i++)
        {
            if (Input.GetKeyUp(observeKeys[i]))
            {
                arg.key = observeKeys[i];
                break;
            }
        }

        ButtonActionQueue.instance.RegisterAction(() => buttonEvent(arg));
    }

    public void OnClick()
    {
        var arg = new InputArg();

        arg.type = InputType.Click;

        for (int i = 0; i < observeKeys.Length; i++)
        {
            if (Input.GetKey(observeKeys[i]))
            {
                arg.key = observeKeys[i];
                break;
            }
        }

        Debug.Log("click");
        ButtonActionQueue.instance.RegisterAction(() => buttonEvent(arg));
    }

    bool die = false;

    //わが子がいなくなったら
    void OnTransformChildrenChanged()
    {
        if (transform.childCount == 0)
        {
            //家に帰る
            instinct.GoHome();
            //gameObject.SetActive(false);
            die = true;
        }
    }

    protected override void OnTransformParentChanged()
    {
        if(die)
        {
            die = false;
            gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (clearEventsOnDisable)
        {
            buttonEvent = nullAction;
        }

        //transform.SetParent(home);
    }

}
