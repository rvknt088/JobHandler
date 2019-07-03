using JobHandler.Entities.Model;
using System.Collections.Generic;

namespace JobHandler.Business.IHelper
{
    public interface IFormatInput
    {
        List<JobsModel> JobsList(List<string> unsortedJobs);

        bool IsValidDependencies(string name, string[] dependencies);
    }
}
