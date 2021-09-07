using Utility;

namespace Actor
{
    //JsonからすべてのCardProfileを作成
    public static class CharacterProfileBuilder
    {

        public static CharacterProfile Get(CharacterName id)
        {
            var data = JsonHelper.GetData<CharacterProfile>((id.ToString()));


            return data;
        }

        public static CharacterProfile[] GetAll()
        {
            var datas = (CharacterName[])System.Enum.GetValues(typeof(CharacterName));
            var instances = new CharacterProfile[datas.Length]; 

            for(int i = 0;i < datas.Length;i++)
            {
                instances[i] = Get(datas[i]);
            }

            return instances;
        }
        
    }
}