using System;
using System.Collections.Generic;

namespace Common
{
    public class SignalBus : ISignalBus
    {
        private readonly HashSet<Type> _currentlyFiring = new HashSet<Type>();

        private readonly Dictionary<Type, List<object>> _subscriptions = new Dictionary<Type, List<object>>();

        private readonly Dictionary<Type, List<object>> _unsubscribeBuffer = new Dictionary<Type, List<object>>();

        public void Fire<T>(T signal)
        {
            var type = typeof(T);
            if (!_subscriptions.TryGetValue(type, out var callbacks)) return;

            _currentlyFiring.Add(type);

            foreach (var obj in callbacks)
                if (obj is Action<T> callback)
                    callback.Invoke(signal);

            if (_unsubscribeBuffer.TryGetValue(type, out var value))
                foreach (var obj in value)
                    callbacks.Remove(obj);

            _currentlyFiring.Remove(typeof(T));
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_subscriptions.ContainsKey(type)) _subscriptions.Add(typeof(T), new List<object>());

            _subscriptions[typeof(T)].Add(callback);
            return new DisposeContainer(() => DisposeCallback<T>(callback));
        }

        public void UnSubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_currentlyFiring.Contains(type)
                && _subscriptions.TryGetValue(type, out var subscription))
            {
                subscription.Remove(callback);
            }
            else
            {
                if (!_unsubscribeBuffer.ContainsKey(type)) _unsubscribeBuffer.Add(type, new List<object>());

                _unsubscribeBuffer[type].Add(callback);
            }
        }

        private void DisposeCallback<T>(object callback)
        {
            if (callback is Action<T> action)
                UnSubscribe(action);
            else
                throw new Exception($"SignalBusService.DisposeCallback: callback is not {typeof(Action<T>)}");
        }
    }

    public interface ISignalBus
    {
        void Fire<T>(T signal);
        IDisposable Subscribe<T>(Action<T> callback);
    }
}