using JobHandler.Business.Helper;
using JobHandler.Business.IHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Collections.Generic;

namespace JobaHandler.Tests
{
    [TestClass]
    public class JobHandlerTest
    {
        private Container sorterContainer;
        private Container formatContainer;
        private IJobSorter _jobSorter;
        private IFormatInput _formatInput;

        [TestInitialize]
        public void Initialize()
        {
            sorterContainer = new Container();

            sorterContainer.Register<IJobSorter, JobSorter>(Lifestyle.Transient);
            _jobSorter = (IJobSorter)sorterContainer.GetInstance(typeof(IJobSorter));

            formatContainer = new Container();

            formatContainer.Register<IFormatInput, FormatInput>(Lifestyle.Transient);
            _formatInput = (IFormatInput)formatContainer.GetInstance(typeof(IFormatInput));
        }

        #region [Problem Statement 1]
        /// <summary>
        /// Success Scenario : Single Job
        /// </summary>
        [TestMethod]
        public void Test01_Single_Job()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            #endregion

            #region ACT
            var unsorted = _formatInput.JobsList(jobs);
            var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
            #endregion

            #region ASSERT
            string resp = string.Empty;
            foreach (var item in sorted)
            {
                resp = string.Format("{0} {1}", resp, item.Name);
            }
            Assert.AreEqual(resp.Trim(), "A");
            #endregion
        }
        #endregion [Problem Statement 1]

        #region [Problem Statement 2]
        /// <summary>
        /// Success Scenario : Three Job
        /// </summary>
        [TestMethod]
        public void Test02_Three_Job()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            jobs.Add("B =>");
            jobs.Add("C =>");

            #endregion

            #region ACT
            var unsorted = _formatInput.JobsList(jobs);
            var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
            #endregion

            #region ASSERT
            string resp = string.Empty;
            foreach (var item in sorted)
            {
                resp = string.Format("{0} {1}", resp, item.Name);
            }
            Assert.AreEqual(resp.Trim(), "A B C");
            #endregion
        }
        #endregion [Problem Statement 2]

        #region [Problem Statement 3]
        /// <summary>
        /// Success Scenario : Three Job With Dependency
        /// </summary>
        [TestMethod]
        public void Test03_Three_Job_With_Dependency()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            jobs.Add("B => C");
            jobs.Add("C =>");
            #endregion

            #region ACT
            var unsorted = _formatInput.JobsList(jobs);
            var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
            #endregion

            #region ASSERT
            string resp = string.Empty;
            foreach (var item in sorted)
            {
                resp = string.Format("{0} {1}", resp, item.Name);
            }
            Assert.AreEqual(resp.Trim(), "A C B");
            #endregion
        }
        #endregion [Problem Statement 3]

        #region [Problem Statement 4]
        /// <summary>
        /// Success Scenario : Six Job With Dependency
        /// </summary>
        [TestMethod]
        public void Test04_Six_Job_With_Dependency()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            jobs.Add("B => C");
            jobs.Add("C => F");
            jobs.Add("D => A");
            jobs.Add("E => B");
            jobs.Add("F =>");
            #endregion

            #region ACT
            var unsorted = _formatInput.JobsList(jobs);
            var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
            #endregion

            #region ASSERT
            string resp = string.Empty;
            foreach (var item in sorted)
            {
                resp = string.Format("{0} {1}", resp, item.Name);
            }
            Assert.AreEqual(resp.Trim(), "A F C B D E");
            #endregion
        }
        #endregion [Problem Statement 4]

        #region [Problem Statement 5]
        /// <summary>
        /// Fail Scenario : Jobs can’t Depend On Themselves
        /// </summary>
        [TestMethod]
        public void Test05_Job_DependOnThemSelf()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            jobs.Add("B =>");
            jobs.Add("C => C");
            #endregion


            try
            {
                #region ACT
                var unsorted = _formatInput.JobsList(jobs);
                var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
                #endregion
            }
            catch (System.Exception ex)
            {
                #region ASSERT
                Assert.AreEqual(ex.Message, "Job can't depend to themself.");
                #endregion
            }

        }
        #endregion [Problem Statement 5]

        #region [Problem Statement 6]
        /// <summary>
        /// Fail Scenario : Jobs can’t have circular dependencies.
        /// </summary>
        [TestMethod]
        public void Test06_Job_Circular_Depedencies()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            jobs.Add("B => C");
            jobs.Add("C => F");
            jobs.Add("D => A");
            jobs.Add("E =>");
            jobs.Add("F => B");
            #endregion

            try
            {
                #region ACT
                var unsorted = _formatInput.JobsList(jobs);
                var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
                #endregion
            }
            catch (System.Exception ex)
            {
                #region ASSERT
                Assert.AreEqual(ex.Message, "Jobs can’t have circular dependencies.");
                #endregion
            }

        }
        #endregion [Problem Statement 6]

        #region [Problem Statement 7]
        /// <summary>
        /// Fail Scenario : Three Job With Dependency
        /// </summary>
        [TestMethod]
        public void Test07_Three_Job_With_Dependency()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            jobs.Add("B => C");
            jobs.Add("C =>");
            #endregion

            #region ACT
            var unsorted = _formatInput.JobsList(jobs);
            var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
            #endregion

            #region ASSERT
            string resp = string.Empty;
            foreach (var item in sorted)
            {
                resp = string.Format("{0} {1}", resp, item.Name);
            }
            Assert.AreNotEqual(resp.Trim(), "A B C");
            #endregion
        }
        #endregion [Problem Statement 7]

        #region [Problem Statement 8]
        /// <summary>
        /// Fail Scenario : Six Job With Dependency
        /// </summary>
        [TestMethod]
        public void Test08_Six_Job_With_Dependency()
        {
            #region ARRANGE
            List<string> jobs = new List<string>();
            jobs.Add("A =>");
            jobs.Add("B => C");
            jobs.Add("C => F");
            jobs.Add("D => A");
            jobs.Add("E => B");
            jobs.Add("F =>");
            #endregion

            #region ACT
            var unsorted = _formatInput.JobsList(jobs);
            var sorted = _jobSorter.Sort(unsorted, x => x.Dependencies, x => x.Name);
            #endregion

            #region ASSERT
            string resp = string.Empty;
            foreach (var item in sorted)
            {
                resp = string.Format("{0} {1}", resp, item.Name);
            }
            Assert.AreNotEqual(resp.Trim(), "A B C D E F");
            #endregion
        }
        #endregion [Problem Statement 8]

    }
}
