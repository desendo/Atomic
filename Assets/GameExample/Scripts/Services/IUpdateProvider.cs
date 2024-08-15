using System;

namespace Game.Services
{
    public interface IUpdateProvider
    {
        public IDisposable Subscribe(Action<float> delta);
        public IDisposable SubscribeFixed(Action<float> delta);
    }
}