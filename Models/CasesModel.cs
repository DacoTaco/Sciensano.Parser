using Sciensano.CovidJson.Parser.SciensanoModels;
using System;

namespace Sciensano.CovidJson.Parser.Models
{
    public class CasesModel : ILocalModel
    {
        public CasesModel() { }
        public CasesModel(SciensanoCasesModel model)
        {
            Date = model.Date;
            Cases = model.Cases;
        }
        public DateTime Date { get; set; }
        public int Cases { get; set; }
        public int TotalCases { get; set; }
    }
}
