using System;
using System.Collections.Generic;

namespace Common
{
    public static class DisposableExt
    {
        public static void AddTo(this IDisposable disposable, List<IDisposable> disposables)
        {
            disposables.Add(disposable);
        }

        public static void DisposeAndClear(this List<IDisposable> disposables)
        {
            foreach (var disposable in disposables) disposable.Dispose();
            disposables.Clear();
        }
    }
}