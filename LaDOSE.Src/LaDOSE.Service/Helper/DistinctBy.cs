using System;
using System.Collections;
using System.Collections.Generic;

namespace LaDOSE.Business.Helper
{
    public static class DataHelper
    {
        public static IEnumerable<T> DistinctBy<T,TKey>(this IEnumerable<T> lst,Func<T,TKey> dist)
        {
            var seen = new HashSet<TKey>();
            foreach (var e in lst)
            {

                if (!seen.Contains(dist(e)))
                {
                    seen.Add(dist(e));
                    yield return e;
                }
            }
        }
    }
}