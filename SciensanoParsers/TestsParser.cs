using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sciensano.CovidJson.Parser.SciensanoParsers
{
    public class TestsParser : BaseSciensanoParser<TestsModel, SciensanoTestsModel>
    {
        public override Task<List<ILocalModel>> ParseAsync(Stream stream)
        {
            //string testJson = "[{\"DATE\":\"2020-03-01\",\"PROVINCE\":\"Antwerpen\",\"REGION\":\"Flanders\",\"TESTS_ALL\":18,\"TESTS_ALL_POS\":0}]";

            var list = new List<TestsModel>();
            var jsonmodels = ParseJson(stream);

            foreach (var model in jsonmodels)
            {

                TestsModel testsModel;
                if (list.Count > 0 && list.Last().Date == model.Date)
                {
                    testsModel = list.Last();
                    testsModel.Positive += model.Tests_All_Pos;
                    testsModel.Tests += model.Tests_all;
                }
                else
                {
                    testsModel = new TestsModel(model);
                }

                testsModel.TotalPositives = list.Sum(x => x.Positive);
                testsModel.TotalTests = list.Sum(x => x.Tests);

                if (!list.Contains(testsModel))
                    list.Add(testsModel);
            }
            return Task.FromResult(list.Cast<ILocalModel>().ToList());
        }
    }
}
