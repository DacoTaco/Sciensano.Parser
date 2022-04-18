using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sciensano.CovidJson.Parser.SciensanoParsers
{
    public class HospitalisationParser : BaseSciensanoParser<HospitalisationModel,SciensanoHospitalisationModel>
    {
        public HospitalisationParser() { }

        public override Task<List<ILocalModel>> ParseAsync(Stream stream)
        {
            //string testJson = "[{\"DATE\":\"2020-03-15\",\"PROVINCE\":\"Antwerpen\",\"REGION\":\"Flanders\",\"NR_REPORTING\":14,\"TOTAL_IN\":50,\"TOTAL_IN_ICU\":9,\"TOTAL_IN_RESP\":4,\"TOTAL_IN_ECMO\":0,\"NEW_IN\":8,\"NEW_OUT\":8}]";

            var list = new List<HospitalisationModel>();
            var jsonmodels = ParseJson(stream);

            foreach (var model in jsonmodels)
            {
                if (list.Count > 0 && list.Last().Date == model.Date)
                {
                    var hospitalisationModel = list.Last();
                    hospitalisationModel.Incoming += model.New_In;
                    hospitalisationModel.Outgoing += model.New_Out;
                    hospitalisationModel.TotalHospitalisations += model.Total_In;
                }
                else
                {
                    list.Add(new HospitalisationModel(model));
                }
            }

            return Task.FromResult(list.Cast<ILocalModel>().ToList());
        }
    }
}
