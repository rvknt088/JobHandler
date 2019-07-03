using JobHandler.Business;
using JobHandler.Business.Common;
using JobHandler.Business.IHelper;
using JobHandler.Entities.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace JobHandler
{
    public class Program
    {

        static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            ///Dependency Injection Call
            HelperDI.Service(collection);

            try
            {
                ///Get Data From User
                int noOfJobs = 0;

                Console.Write("Enter the no. of jobs to schedule: \n");

                int.TryParse(Console.ReadLine(), out noOfJobs);

                List<string> jobs = new List<string>();

                int i = 0;
                ///Get List of Jobs
                while (i < noOfJobs)
                {
                    Console.Write(string.Format("{0} {1} {2}: \n", "Enter the job", i + 1, "and press enter"));

                    string input = Console.ReadLine();

                    bool isValidated = input.ValidateInput();

                    if (!isValidated)
                    {
                        Console.WriteLine("Invalid Input");
                    }
                    else
                    {
                        jobs.Add(input.Trim());
                        i++;
                    }
                }
                IServiceProvider serviceProvider = collection.BuildServiceProvider();
                //format job list
                var _formatInput = serviceProvider.GetService<IFormatInput>();
                var unsorted = _formatInput.JobsList(jobs);
                //sort job list
                var _jobSorter = serviceProvider.GetService<IJobSorter>();
                var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
                
                #region Print
                Console.WriteLine("Sorted jobs are: ");
                //print sorted job
                foreach (var item in sorted)
                {
                    Console.Write(item.Name);
                    Console.Write(" ");
                }
                Console.WriteLine();
                #endregion Print

                if (serviceProvider is IDisposable)
                {
                    ((IDisposable)serviceProvider).Dispose();
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

    }
}
