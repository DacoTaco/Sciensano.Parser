using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Sciensano.CovidJson.Parser.SciensanoParsers
{
    public class CasesParser : BaseSciensanoParser<CasesModel, SciensanoCasesModel>
    {
        public override Task<List<ILocalModel>> ParseAsync(Stream stream)
        {
            //string testJson = "[{\"DATE\":\"2020-03-01\",\"PROVINCE\":\"Antwerpen\",\"REGION\":\"Flanders\",\"AGEGROUP\":\"40-49\",\"SEX\":\"M\",\"CASES\":1}]";
            var list = new List<CasesModel>();
            var jsonmodels = ParseJson(stream);

            foreach (var model in jsonmodels)
            {
                if (!model.Date.HasValue)
                    continue;

                CasesModel casesModel;
                if (list.Count > 0 && list.Last().Date == (model.Date ?? new DateTime()))
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
            return Task.FromResult(list.Cast<ILocalModel>().ToList());
        }
    }
}
