using System;

namespace Sciensano.CovidJson.Parser.SciensanoModels
{
    public class SciensanoHospitalisationModel : IDatedSciensanoModel
    {
        public DateTime? Date { get; set; }
        public int New_In { get; set; }
        public int New_Out { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public int NR_Reporting { get; set; }
        public int Total_In { get; set; }
        public int Total_In_Icu { get; set; }
        public int Total_In_Resp { get; set; }
        public int Total_In_Ecmo { get; set; }       
    }
}
