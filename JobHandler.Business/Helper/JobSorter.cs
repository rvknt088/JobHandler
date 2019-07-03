using JobHandler.Business.IHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JobHandler.Business.Helper
{
    public class JobSorter : IJobSorter
    {
        #region Job Sorter
        /// <summary>
        /// Sort
        /// </summary>
        /// <typeparam name="T">Type of Enumerable</typeparam>
        /// <typeparam name="TKey">Type of Enumerable</typeparam>
        /// <param name="source">Source</param>
        /// <param name="getDependencies">Dpendencies</param>
        /// <param name="getKey">Get key</param>
        /// <returns></returns>
        public IList<T> Sort<T, TKey>(IEnumerable<T> source, Func<T, IEnumerable<TKey>> getDependencies, Func<T, TKey> getKey)
        {
            ICollection<T> source2 = (source as ICollection<T>) ?? source.ToArray();
            return Sort<T>(source2, RemapDependencies(source2, getDependencies, getKey), null);
        }

        /// <summary>
        /// Remop Dependecies
        /// </summary>
        /// <typeparam name="T">Type of Enumerable</typeparam>
        /// <typeparam name="TKey">Type of Enumerable</typeparam>
        /// <param name="source">Source</param>
        /// <param name="getDependencies">Dpendencies</param>
        /// <param name="getKey">Get key</param>
        /// <returns></returns>
        private Func<T, IEnumerable<T>> RemapDependencies<T, TKey>(IEnumerable<T> source, Func<T, IEnumerable<TKey>> getDependencies, Func<T, TKey> getKey)
        {
            var map = source.ToDictionary(getKey);
            return item =>
            {
                var dependencies = getDependencies(item);
                return dependencies != null
                    ? dependencies.Select(key => map[key])
                    : null;
            };
        }

        /// <summary>
        /// Sort
        /// </summary>
        /// <typeparam name="T">Type of Enumerable (Name)</typeparam>
        /// <param name="source">Source</param>
        /// <param name="getDependencies">Get Dpendencies</param>
        /// <param name="getKey">Get key</param>
        /// <returns></returns>
        private IList<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies, IEqualityComparer<T> comparer = null)
        {
            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>(comparer);

            foreach (var item in source)
            {
                Visit(item, getDependencies, sorted, visited);
            }

            return sorted;
        }

        /// <summary>
        /// Visit
        /// </summary>
        /// <typeparam name="T">Type of Enumerable (Name)</typeparam>
        /// <param name="item">Item</param>
        /// <param name="getDependencies">Get Dpendencies</param>
        /// <param name="getKey">Get key</param>
        /// <param name="visited">Check if Visited</param>
        private void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Jobs can’t have circular dependencies.");
                }
            }
            else
            {
                visited[item] = true;

                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        Visit(dependency, getDependencies, sorted, visited);
                    }
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }

        #endregion Job Sorter
    }
}
