using JobHandler.Business.Helper;
using JobHandler.Business.IHelper;
using Microsoft.Extensions.DependencyInjection;

namespace JobHandler.Business
{
    public class HelperDI
    {
        public static void Service(IServiceCollection services)
        {
            services.AddTransient<IFormatInput, FormatInput>();
            services.AddTransient<IJobSorter, JobSorter>();
        }
    }
}
