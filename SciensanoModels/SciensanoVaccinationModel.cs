using System;

namespace Sciensano.CovidJson.Parser.SciensanoModels
{
    //{"DATE":"2020-12-28","REGION":"Brussels","AGEGROUP":"25-34","SEX":"F","BRAND":"Pfizer-BioNTech","DOSE":"A","COUNT":1}
    public class SciensanoVaccinationModel : IDatedSciensanoModel
    {
        public DateTime? Date { get; set; }
        public string Region { get; set; }
        public string AgeGroup { get; set; }
        public string Sex { get; set; }
        public string Brand { get; set; }
        public Dose Dose { get; set; }
        public int Count { get; set; }
    }

    public enum Dose
    {
        Unknown,
        A,
        B,
        C,
        E,
        E2
    }
}
