using Utility;

namespace CardSystem
{
    //JsonからすべてのCardProfileを作成
    public static class CardProfileBuilder
    {
        public static CardViewProfile Get(CardName id)
        {
            var name = id.ToString();
            var data = JsonHelper.GetData<CardViewProfile>((name));

            return data;
        }

        public static CardViewProfile[] GetAll()
        {
            var datas = (CardName[])System.Enum.GetValues(typeof(CardName));
            var instances = new CardViewProfile[datas.Length]; 

            for(int i = 0;i < datas.Length;i++)
            {
                instances[i] = Get(datas[i]);
            }

            return instances;
        }
        
    }
}