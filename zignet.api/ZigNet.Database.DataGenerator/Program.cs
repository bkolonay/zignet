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
                new DummyTestModel { Name = "SEO search for listings at the city level for a standard property type", Categories = new string[] { "Listing", "ProdSafe" } }, // 23
                new DummyTestModel { Name = "Search for lease listings via non-company based LoopLinks", Categories = new string[] { "ListingMarketplace", "ProdSafe" } },
                new DummyTestModel { Name = "Retrieve users who are watching multi listing ids using POST request", Categories = new string[] { "Watchlistings", "ProdSafe" } },
                new DummyTestModel { Name = "Delete a watched listing from a FS folder for a mobile user", Categories = new string[] { "Mobile-Watchlistings", "ProdSafe" } },
                new DummyTestModel { Name = "I update the targetedUserExperience", Categories = new string[] { "SavedSearch", "ProdSafe" } },
                new DummyTestModel { Name = "Update a FL save search for a specific user with a save search ID", Categories = new string[] { "SavedSearch", "ProdSafe" } },
                new DummyTestModel { Name = "Update notification message to read", Categories = new string[] { "ListingAlert", "ProdSafe" } },
                new DummyTestModel { Name = "Get a matrix of analytical data keyed around one or more SEO property type IDs for NY", Categories = new string[] { "ListingMarketplace" } },
                new DummyTestModel { Name = "Create new FS watch listing folder on mobile", Categories = new string[] { "ListingMarketplace", "ProdSafe" } },
                new DummyTestModel { Name = "Search for listings by pn 2", Categories = new string[] {"ListingAdvertising", "ProdSafe" } }, // 22
            };

            var zignetApiHandler = new ZigNetApiHandler(new HttpRequestSender(), "http://localhost:52529/");

            for (var i = 0; i < 1; i++)
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
