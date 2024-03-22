using System;
using System.Collections.Generic;

namespace LaDOSE.DesktopApp.Avalonia.Utils
{
    public static class CustomListExtension
    {
        public sealed class EqualityComparer<T> : IEqualityComparer<T> where T : class
        {
            private readonly Func<T, T, bool> _compare;

            public EqualityComparer(Func<T, T, bool> c)
            {
                _compare = c;
            }

            public bool Equals(T? x, T? y)
            {
                return _compare(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
