using Sciensano.CovidJson.Parser.SciensanoModels;
using System;

namespace Sciensano.CovidJson.Parser.Models
{
    public class TestsModel : ILocalModel
    {
        public TestsModel() { }
        public TestsModel(SciensanoTestsModel model)
        {
            Date = model.Date;
            Tests = model.Tests_all;
            Positive = model.Tests_All_Pos;
        }
        public DateTime Date { get; set; }
        public int Positive { get; set; }
        public int TotalPositives { get; set; }
        public int Tests { get; set; }
        public int TotalTests { get; set; }
    }
}
