using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Effects;

namespace CardSystem
{
    //カードの見た目。
    //テキストの配置や
    //ボタンを押したときの変化も含む見た目を決める
    //これは意味を伴わないのでローカルでの処理を行う。
    [System.Serializable]
    public class CardFormatter
    {
        [SerializeField] Sprite frameSprite;
        [SerializeField] TMP_Text namePlace;
        [SerializeField] TMP_Text summaryPlace;

        [SerializeField] GUIEffectName pushedEffect = GUIEffectName.card_push_down;
        [SerializeField] GUIEffectName hoverEffect = GUIEffectName.card_hover;
        [SerializeField] GUIEffectName setDefaultEffect = GUIEffectName.card_back_to_start;
        //ObjectEffectに渡すのでそれぞれGOを分けること。
        [SerializeField] Image frameRenderer;
        [SerializeField] Image thumbNailRenderer;

        public Rect cardRect{get{return frameRenderer.rectTransform.rect;}}
        Card target;

        //カードをセット。(シリアライズするのでコンストラクタは呼び出せない)
        public void Load(Card target)
        {
            this.target = target;
            if (frameRenderer.sprite != frameSprite)
            {
                frameRenderer.sprite = frameSprite;
            }
        }

        //カード情報の再読み込み
        public void Format()
        {
            namePlace.text = target.name.ToString();
            summaryPlace.text = target.summary;
            thumbNailRenderer.sprite = target.thumbnail;
        }

        //ボタンにこれを登録する。
        public void FormatCallback(InputArg arg)
        {
            IVisualEffect effect = null;

            if (arg.type == InputType.pointerDown)
            {
                effect = EffectServer.instance.GetGUIMotion(pushedEffect, frameRenderer);
            }
            else if (arg.type == InputType.hoverStart)
            {
                effect = EffectServer.instance.GetGUIMotion(hoverEffect, frameRenderer);
            }
            else if (arg.type == InputType.pointerUp || arg.type == InputType.hoverEnd)
            {
                effect = EffectServer.instance.GetGUIMotion(setDefaultEffect,frameRenderer);
            }

            if(effect != null)
            {
                GameManager.instance.RegisterGUIMotion(effect);
            }
        }
    }
}