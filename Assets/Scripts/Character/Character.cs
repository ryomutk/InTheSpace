using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CardSystem;
using System.Collections;

namespace Actor
{
    [RequireComponent(typeof(Animator), typeof(Image))]
    public class Character : MonoBehaviour
    {
        new public CharacterName name { get; set; }
        CharacterStatus _status;

        /// <summary>
        /// 立ち絵を表示するためのレンダラ。
        /// なければnull
        /// </summary>
        public Image viewRenderer { get; private set; }
        public Animator animator { get; private set; }
        public CharacterStatus Status { get { return _status; } }

        //public List<CardSystem.CardViewProfile> cardList { get; set; }
        public event CharacterAction OnEnter;
        public event CharacterAction OnExit;


        public virtual void Enter()
        {
            OnEnter(this);
        }

        public virtual void Exit()
        {
            OnExit(this);
        }

        void OnDisable()
        {
            OnEnter = (x) => { };
            OnExit = (x) => { };
        }


        protected void Awake()
        {
            OnEnter = (x) => { };
            OnExit = (x) => { };

            _status = new CharacterStatus(this);
            viewRenderer = GetComponent<Image>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            GameManager.instance.onGameEvent += (x) =>
            {
                if (x == GameState.systemInitialize)
                {
                    Debug.Log("呼ばれている");
                    BattleManager.instance.OnBattleEvent += (x) => OnBattleEvent(x);
                }
            };
        }

        void OnBattleEvent(BattleState state)
        {
            if (state == BattleState.dataInitialize)
            {

                if (gameObject.activeSelf)
                {
                    var task = new InteraptTask();
                    BattleManager.instance.RegisterInterapt(task);

                    StartCoroutine(PrepareForBattle(task));
                }
            }
        }

        IEnumerator PrepareForBattle(InteraptTask task)
        {
            //今はまだ特に意味なし。重い処理がないので
            yield return null;
            task.finished = true;
        }
    }
}