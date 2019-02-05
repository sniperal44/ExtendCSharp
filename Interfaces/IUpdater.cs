namespace ExtendCSharp.Interfaces
{
    public delegate void Updated(object self);
    public interface IUpdater
    {

        event Updated OnUpdated;
    }
}
