using Actor;

public class Engine : RocketEquipment
{
    public override bool upgradeable { get { return true; } }
    public override string discription { get { return ""; } }
    protected virtual string engineString { get { return "ごく普通のエンジン。"; } }
    string discriptionFormat = "{0}消費エネルギー:{1}/s 推進力:{2}/s";
    //一応ターゲット選べるようにするけど、まぁPlayer以外に使う予定はない。
    public Engine(Character target = null)
    {
        if (target == null)
        {

        }
    }

    string Discription()
    {

    }

}