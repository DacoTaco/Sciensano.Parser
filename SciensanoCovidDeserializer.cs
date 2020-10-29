using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sciensano.CovidJson.Parser
{
    public static class SciensanoCovidDeserializer
    {
        private static IEnumerable<T> ParseJsonFile<T>(string filePath) where T : ISciensanoModel
        {
            var list = new List<T>();
            if (!File.Exists(filePath))
                return list.AsEnumerable();

            JsonSerializer serializer = new JsonSerializer();
            using (FileStream s = File.Open(filePath, FileMode.Open))
            //using (MemoryStream s = new MemoryStream(Encoding.ASCII.GetBytes(testJson)))
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    // deserialize only when there's "{" character in the stream
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var _object = serializer.Deserialize<T>(reader);
                        list.Add(_object);
                    }
                }
            }

            if (typeof(T).GetInterfaces().Contains(typeof(IDatedSciensanoModel)))
            {
                list = list.Cast<IDatedSciensanoModel>().OrderBy(x => x.Date).Cast<T>().ToList();
            }

            return list.AsEnumerable();
        }

        public static List<HospitalisationModel> GetHospitalisation(string filePath)
        {
            string testJson = "[{\"DATE\":\"2020-03-15\",\"PROVINCE\":\"Antwerpen\",\"REGION\":\"Flanders\",\"NR_REPORTING\":14,\"TOTAL_IN\":50,\"TOTAL_IN_ICU\":9,\"TOTAL_IN_RESP\":4,\"TOTAL_IN_ECMO\":0,\"NEW_IN\":8,\"NEW_OUT\":8}]";

            var list = new List<HospitalisationModel>();
            var jsonmodels = ParseJsonFile<SciensanoHospitalisationModel>(filePath);

            foreach(var model in jsonmodels)
            {
                HospitalisationModel hospitalisationModel;

                if (list.Count > 0 && list.Last().Date == model.Date)
                {
                    hospitalisationModel = list.Last();
                    hospitalisationModel.Incoming += model.New_In;
                    hospitalisationModel.Outgoing += model.New_Out;
                }
                else
                {
                    hospitalisationModel = new HospitalisationModel(model);
                    hospitalisationModel.PreviousTotalHospitalisations = list.LastOrDefault()?.TotalHospitalisations ?? 0;
                }

                if (!list.Contains(hospitalisationModel))
                    list.Add(hospitalisationModel);
            }

            return list;
        }

        public static List<CasesModel> GetCases(string filePath)
        {
            string testJson = "[{\"DATE\":\"2020-03-01\",\"PROVINCE\":\"Antwerpen\",\"REGION\":\"Flanders\",\"AGEGROUP\":\"40-49\",\"SEX\":\"M\",\"CASES\":1}]";
            var list = new List<CasesModel>();
            var jsonmodels = ParseJsonFile<SciensanoCasesModel>(filePath);

            foreach (var model in jsonmodels)
            {
                CasesModel casesModel;
                if (list.Count > 0 && list.Last().Date == model.Date)
                {
                    casesModel = list.Last();
                    casesModel.Cases += model.Cases;
                }
                else
                {
                    casesModel = new CasesModel(model);
                }

                casesModel.TotalCases = list.Sum(x => x.Cases);

                if (!list.Contains(casesModel))
                    list.Add(casesModel);
            }
            return list;
        }

        public static List<TestsModel> GetTests(string filePath)
        {
            string testJson = "[{\"DATE\":\"2020-03-01\",\"PROVINCE\":\"Antwerpen\",\"REGION\":\"Flanders\",\"TESTS_ALL\":18,\"TESTS_ALL_POS\":0}]";

            var list = new List<TestsModel>();
            var jsonmodels = ParseJsonFile<SciensanoTestsModel>(filePath);

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

                if(!list.Contains(testsModel))
                    list.Add(testsModel);
            }
            return list;
        }
    }
}
