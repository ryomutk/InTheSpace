using UnityEngine;
using System;

namespace Effects
{
    public interface IVisualEffect
    {
        bool compleated{get;}
        void Execute(RendererGetter rendererGetter, AudioSource audio);
        bool dontDisturb { get; }
    }
}