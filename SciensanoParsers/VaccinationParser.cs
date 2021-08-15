using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sciensano.CovidJson.Parser.SciensanoParsers
{
    public class VaccinationParser : BaseSciensanoParser<VaccinationModel, SciensanoVaccinationModel>
    {
        public override Task<List<ILocalModel>> ParseAsync(Stream stream)
        {
            var list = new List<VaccinationModel>();
            var jsonmodels = ParseJson(stream);

            var doses = jsonmodels.Select(m => new { m.Dose, m.Brand }).Distinct();

            foreach(var dose in doses)
            {
                var model = new VaccinationModel(jsonmodels.First(m => m.Brand == dose.Brand && m.Dose == dose.Dose))
                {
                    Date = DateTime.Now,
                    Count = jsonmodels.Where(m => m.Brand == dose.Brand && m.Dose == dose.Dose).Sum(m => m.Count)
                };

                if(model.Dose == Models.Dose.First)
                {
                    model.Count -= jsonmodels.Where(m => m.Brand == dose.Brand && m.Dose == SciensanoModels.Dose.B).Sum(m => m.Count);
                }

                list.Add(model);
            }

            return Task.FromResult(list.Cast<ILocalModel>().ToList());
        }
    }
}
