public class InteraptTask:IInteraptor
{
    public bool finished{get;set;}
    public InteraptTask()
    {
        finished = false;
    }

    public InteraptTask (IInteraptHandler handler)
    {
        finished = false;
        handler.RegisterInterapt(this);
    }
}