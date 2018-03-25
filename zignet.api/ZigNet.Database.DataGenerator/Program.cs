using System;
using System.Collections.Generic;
using ZigNet.Adapter;
using ZigNet.Adapter.Http;
using ZigNet.Api.Model;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Database.DataGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            var tests = new List<DummyTestModel>
            {
                new DummyTestModel { Name = "Search for brokers", Categories = new string[] {"Broker", "ProdSafe" } }, // 13
                new DummyTestModel { Name = "Get walk score", Categories = new string[] {"Location", "ProdSafe" } },
                new DummyTestModel { Name = "Get traffic", Categories = new string[] {"Location", "ProdSafe" } },
                new DummyTestModel { Name = "Delete a single watch listing in FL watch folder", Categories = new string[] {"Watchlistings", "ProdSafe" } },
                new DummyTestModel { Name = "Get user listing alert option", Categories = new string[] {"ListingAlert", "ProdSafe" } },
                new DummyTestModel { Name = "Get impression by contactIdAsync", Categories = new string[] {"ListingAnalytics", "ProdSafe" } },
                new DummyTestModel { Name = "Get new listings on page 1", Categories = new string[] {"ListingAlert", "ProdSafe" } },
                new DummyTestModel { Name = "Mark mobile lead as read", Categories = new string[] {"Mobile-Leads" } },
                new DummyTestModel { Name = "Get notification messages for user", Categories = new string[] {"ListingAlert", "ProdSafe" } },
                new DummyTestModel { Name = "Update user listing alert", Categories = new string[] {"ListingAlert", "ProdSafe" } }, // 22
            };

            var zignetApiHandler = new ZigNetApiHandler(new HttpRequestSender(), "http://localhost:84/");

            for (var i = 0; i < 300000; i++)
            {
                Console.WriteLine("Iteration: " + i);
                var suiteResultId = zignetApiHandler.StartSuite(26);
                Console.WriteLine(string.Format("Started SuiteResultID: {0}", suiteResultId));

                var random = new Random();
                foreach (var test in tests)
                {
                    var createTestResultModel = new CreateTestResultModel
                    {
                        SuiteResultId = suiteResultId,
                        TestName = test.Name,
                        TestCategories = test.Categories,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow.AddSeconds(30),
                        TestResultType = GetRandomTestResultType(random)
                    };

                    if (createTestResultModel.TestResultType == TestResultType.Fail)
                    {
                        createTestResultModel.TestFailureType = GetRandomTestFailureType(random);
                        createTestResultModel.TestFailureDetails = "dummy test failure details";
                    }
                    zignetApiHandler.SaveTestResult(createTestResultModel);
                }

                zignetApiHandler.StopSuite(suiteResultId, SuiteResultType.Inconclusive);
            }
            Console.WriteLine("DONE!");
            Console.WriteLine(string.Format("Start Time: {0} {1}", startTime.ToShortDateString(), startTime.ToShortTimeString()));
            Console.WriteLine(string.Format("End Time {0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
        }

        private static TestResultType GetRandomTestResultType(Random random)
        {
            int num = random.Next(1, 3);
            return (TestResultType)num;
        }

        private static TestFailureType GetRandomTestFailureType(Random random)
        {
            int num = random.Next(0, 2);
            return (TestFailureType)num;
        }
    }
}
