using UnityEngine;
using System.Collections;

[System.Serializable]
public struct TextureData
{
    [SerializeField] byte[] _data;
    [SerializeField] Vector2 _pivot;
    [SerializeField] float _pixPerUnit;
    [SerializeField] Vector2 _size;
    [SerializeField] TextureFormat _format;
    public byte[] data{get{return _data;}set{_data = value;}}
    public TextureFormat format{get{return _format;}set{_format=value;}}
    public Vector2 size{get{return _size;}set{_size = value;}}
    public Vector2 pivot{get{return _pivot;}set{_pivot = value;}}
    public float pixelsPerUnit{get{return _pixPerUnit;}set{_pixPerUnit = value;}}
}



