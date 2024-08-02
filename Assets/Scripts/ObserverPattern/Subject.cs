using System.Collections.Generic;

namespace ObserverPattern
{
    public static class Subject
    {
        private static Dictionary<EventKey, HashSet<IObserver>> _observers = new();

        private static List<IObserver> cacheObservers = new();

        public static void Register(IObserver observer, EventKey key)
        {
            if (!_observers.ContainsKey(key)) _observers[key] = new HashSet<IObserver>();
            if (_observers[key].Contains(observer)) return;
            _observers[key].Add(observer);
        }

        public static void Unregister(IObserver observer, EventKey key)
        {
            if (!_observers.ContainsKey(key)) return;
            if (!_observers[key].Contains(observer)) return;
            _observers[key].Remove(observer);
        }

        public static void Notify(EventKey key)
        {
            if (!_observers.ContainsKey(key)) return;
            // Debug.Log(key);
            cacheObservers.Clear();
            cacheObservers.AddRange(_observers[key]);
            foreach (var observer in cacheObservers)
            {
                observer.OnNotify(key);
            }
        }
        
        public static void Notify(EventKey key, params object[] args)
        {
            if (!_observers.ContainsKey(key)) return;
            // Debug.Log(key);
            cacheObservers.Clear();
            cacheObservers.AddRange(_observers[key]);
            foreach (var observer in cacheObservers)
            {
                observer.OnNotify(key, args);
            }
        }
    }
}