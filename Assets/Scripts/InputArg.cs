using UnityEngine;

public struct InputArg
{
    public KeyCode key{get;set;}
    public InputType type{get;set;}
}

public enum InputType
{
    none,
    pointerDown,
    pointerUp,
    Click,
    hoverStart,
    hoverEnd
}