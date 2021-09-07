using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

namespace Effects
{
    [CreateAssetMenu(menuName = "ObjectEffect/Character/CharacterMove")]
    //キャラのとるワンステップモーション
    public class CharacterMoveMotionData :ObjectEffectData
    {
        public override ObjEffectName name{get{return ObjEffectName.character_move;}}
        [SerializeField] CharacterMoveMotion _base;
        protected override ObjectEffect cloneBase{get{return _base;}}

        [System.Serializable]
        class CharacterMoveMotion:ObjectEffect
        {
            AnimatorOverrideController overrideAnimator;
            [SerializeField] string triggerName = "Move";
            Actor.CharacterInSpace targetCharacter;
            public override bool compleated{get;protected set;}
            
            public CharacterMoveMotion():base(false)
            {}

            public override void SetTarget(MonoBehaviour target)
            {
                base.SetTarget(target);
                if(target is Actor.CharacterInSpace chara)
                {
                    targetCharacter = chara;
                    overrideAnimator = AnimationServer.instance.GetOverrideController(chara.name);
                    chara.animator.runtimeAnimatorController = overrideAnimator;
                }
                else
                {
                    Debug.LogWarning("I only can handle nowhere Character!");
                }
            }

            public override void Execute(RendererGetter rendererGetter, AudioSource audioSource)
            {
                targetCharacter.animator.SetTrigger(triggerName);
                var info = targetCharacter.animator.GetNextAnimatorStateInfo(0);
                var duration = info.length;
                var size = BattleManager.instance.nowSession.GetStepSize(targetCharacter);
                
                var tw = targetCharacter.transform.DOMoveX(-size,duration).SetRelative();

                tw.onComplete += () => compleated = true;

                tw.Play();
            }
            
        } 
    }
}