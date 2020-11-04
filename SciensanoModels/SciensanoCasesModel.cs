using System;

namespace Sciensano.CovidJson.Parser.SciensanoModels
{
    //{"DATE":"2020-03-01","PROVINCE":"Antwerpen","REGION":"Flanders","AGEGROUP":"40-49","SEX":"M","CASES":1}
    public class SciensanoCasesModel : IDatedSciensanoModel
    {
        public DateTime? Date { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string AgeGroup { get; set; }
        public string Sex { get; set; }
        public int Cases { get; set; }
    }
}
