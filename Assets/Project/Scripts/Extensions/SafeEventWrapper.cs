using System;

namespace Extensions
{
    public class SafeEventWrapper
    {
        public event Action Event;

        public void Invoke() =>
            Event?.Invoke();
    }
    
    public class SafeAction<T>
    {
        public event Action<T> Action;

        public void Invoke(T t) =>
            Action?.Invoke(t);
    }
}