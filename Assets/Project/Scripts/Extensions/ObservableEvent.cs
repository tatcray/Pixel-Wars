using System;

namespace Extensions
{
    public class ObservableEvent
    {
        public Action Event;

        public void Invoke()
        {
            Event?.Invoke();
        }
    }
    
    public class ObservableEvent<T>
    {
        public Action<T> Event;

        public void Invoke(T t)
        {
            Event?.Invoke(t);
        }
    }
}