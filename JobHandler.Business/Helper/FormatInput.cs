using JobHandler.Business.IHelper;
using JobHandler.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobHandler.Business.Helper
{
    public class FormatInput : IFormatInput
    {
        #region [JobsList]

        /// <summary>
        /// Job List
        /// </summary>
        /// <param name="unsortedJobs">unsorted job</param>
        /// <returns>formated job list</returns>
        public List<JobsModel> JobsList(List<string> unsortedJobs)
        {
            var unsorted = new List<JobsModel>();
            foreach (var item in unsortedJobs)
            {
                string[] job = item.Split("=>", StringSplitOptions.RemoveEmptyEntries);
                var name = job[0].Trim();
                var depedecies = job.Select(x => x == null ? null : x.Trim()).Where((source, index) => index != 0).ToArray();
                if(!IsValidDependencies(name, depedecies))
                {
                    throw new ArgumentException("Job can't depend to themself.");
                }
                unsorted.Add(new JobsModel(name, depedecies));
            }
            return unsorted;
        }
        #endregion [JobsList]

        #region [Validate Dependencies]
        /// <summary>
        /// Validate Dependencies
        /// Job can't depend to themself
        /// </summary>
        /// <param name="name">Job</param>
        /// <param name="dependencies">Dependent Job</param>
        /// <returns>true/false</returns>
        public bool IsValidDependencies(string name, string[] dependencies)
        {
            if (dependencies.Contains(name))
                return false;
            else
                return true;
        }
        #endregion [Validate Dependencies]
    }
}
