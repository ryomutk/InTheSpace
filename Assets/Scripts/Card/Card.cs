using UnityEngine;
using Effects;
using Actor;
namespace CardSystem
{
    public class Card : MonoBehaviour
    {
        public new CardName name { get { return profile.name; } }
        public CharacterStates statusRequirement { get{return profile.statusRequirement;} }
        public AbilityAttribute attribute { get { return profile.attribute; } }
        public int defaultWeight { get { return profile.defaultWeight; } }
        public Sprite thumbnail { get { return profile.thumbnail; } }
        public string flavorText { get { return profile.flavorText; } }
        public string summary { get { return profile.summary; } }
        [SerializeField] CardFormatter _formatter;
        public CardFormatter formatter{get{return _formatter;}private set{_formatter = value;}}


        public event System.Action<Card, CardEventName> onCardEvent;



        //Characterと違ってこれはゲーム間でもあまり変わらないので
        //一つ一つではなくProfileを入力してもらう
        public CardViewProfile profile { get; set; }
        CardActionBase action;

        protected void Awake()
        {
            formatter.Load(this);
            //null回避
            onCardEvent += (x,y) => {};
        }

        public void Initialize(CardViewProfile profile, CardActionBase action)
        {
            this.profile = profile;
            this.action = action;
            profile.LoadToCard(this);
            formatter.Format();
        }

        public void Enter()
        {
            onCardEvent(this, CardEventName.entered);
        }

        public void Exit()
        {
            onCardEvent(this, CardEventName.exited);
        }

        public void Select(bool selected)
        {
            if (selected)
            {

                onCardEvent(this, CardEventName.selected);
            }
            else
            {
                onCardEvent(this,CardEventName.disSelected);
            }
        }

        public string GetDetail(Character master)
        {
            return action.GetDetail(master);
        }

        public bool Use(Character master)
        {
            onCardEvent(this,CardEventName.used);

            if(action.Playable)
            {
                action.Execute(master);

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void OnDisable()
        {
            onCardEvent(this,CardEventName.disabled);
            onCardEvent =(x,y)=> {};
        }

        public void ButtonCallback(InputArg arg)
        {
            formatter.FormatCallback(arg);
        }

    }
}