using System;
using System.Collections.Generic;

namespace Scripts.GameEventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> EventCallbacks = new();


        public static void Subscribe<TEvent>(Action<TEvent> callback) where TEvent : class
        {
            var type = typeof(TEvent);
            if (EventCallbacks.TryGetValue(type, out var value))
            {
                EventCallbacks[type] = Delegate.Combine(value, callback);
            }
            else
            {
                EventCallbacks[type] = callback;
            }
        }

        public static void Unsubscribe<TEvent>(Action<TEvent> callback) where TEvent : class
        {
            var type = typeof(TEvent);
            if (EventCallbacks.TryGetValue(type, out var value))
            {
                var updated = Delegate.Remove(value, callback);
                if (updated == null)
                {
                    EventCallbacks.Remove(type);
                }
                else
                {
                    EventCallbacks[type] = updated;
                }
            }
        }

        public static void Raise<TEvent>(TEvent eventData) where TEvent : class
        {
            var type = typeof(TEvent);
            if (EventCallbacks.TryGetValue(type, out var value))
            {
                ((Action<TEvent>)value)?.Invoke(eventData);
            }
        }
    }
}