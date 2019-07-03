using System;
using System.Collections.Generic;

namespace JobHandler.Business.IHelper
{
    public interface IJobSorter
    {
        IList<T> Sort<T, TKey>(IEnumerable<T> source, Func<T, IEnumerable<TKey>> getDependencies, Func<T, TKey> getKey);
    }
}
