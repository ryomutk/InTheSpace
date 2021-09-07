using UnityEngine;

namespace Effects
{
    //各種エフェクトが見つからなかった時のダミー
    public class NullEffect : IVisualEffect
    {
        public bool compleated { get{return true;} }
        public void Execute(RendererGetter rendererGetter, AudioSource audio)
        {}
        public bool dontDisturb { get{return false;}}
    }
}