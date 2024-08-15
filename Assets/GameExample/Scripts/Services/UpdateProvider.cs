using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Services
{
    public class UpdateProvider : IUpdateProvider, IUpdate
    {
        private readonly List<Action<float>> _callbacks = new List<Action<float>>();
        private readonly List<Action<float>> _fixedCallbacks = new List<Action<float>>();

        public IDisposable Subscribe(Action<float> onDelta)
        {
            _callbacks.Add(onDelta);

            return new DisposeContainer(()=> Unsubscribe(onDelta));
        }
        public IDisposable SubscribeFixed(Action<float> onDelta)
        {
            _fixedCallbacks.Add(onDelta);
            return new DisposeContainer(()=> UnsubscribeFixed(onDelta));
        }
        private void Unsubscribe(Action<float> onDelta)
        {
            _callbacks.Remove(onDelta);
        }
        private void UnsubscribeFixed(Action<float> onDelta)
        {
            _fixedCallbacks.Remove(onDelta);
        }
        public void Update(float dt)
        {
            foreach (var callback in _callbacks)
            {

                callback.Invoke(dt);
            }
        }
        public void FixedUpdate(float dt)
        {
            foreach (var callback in _fixedCallbacks)
            {
                callback.Invoke(dt);
            }
        }
    }
}