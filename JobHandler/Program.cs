using JobHandler.Business.Helper;
using System;
using System.Collections.Generic;

namespace JobHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ///Get Data From User
                int noOfJobs = 0;

                Console.Write("Enter the no. of jobs to schedule: \n");

                int.TryParse(Console.ReadLine(), out noOfJobs);

                List<string> jobs = new List<string>();

                int i = 0;
                while (i < noOfJobs)
                {
                    Console.Write(string.Format("{0} {1} {2}: \n", "Enter the job", i + 1, "and press enter"));

                    string input = Console.ReadLine();

                    bool isValidated = InputValidator.ValidateInput(input);

                    if (!isValidated)
                    {
                        Console.WriteLine("Invalid Input");
                    }
                    else
                    {
                        Console.WriteLine("Valid Input");
                        i++;
                    }
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
