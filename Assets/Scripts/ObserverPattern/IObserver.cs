namespace ObserverPattern
{
    public interface IObserver
    {
        void OnNotify(EventKey key, params object[] args);
    }
}