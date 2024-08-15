using System;

namespace Common
{
    public class DisposeContainer : IDisposable
    {
        private Action _disposeAction;

        public DisposeContainer(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public DisposeContainer()
        {
        }

        public void Dispose()
        {
            _disposeAction?.Invoke();
        }

        public void SetDisposeAction(Action action)
        {
            _disposeAction = action;
        }
    }
}