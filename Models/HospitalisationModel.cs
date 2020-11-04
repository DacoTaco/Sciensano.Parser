using Sciensano.CovidJson.Parser.SciensanoModels;
using System;

namespace Sciensano.CovidJson.Parser.Models
{
    public class HospitalisationModel : ILocalModel
    {
        public HospitalisationModel() { }
        public HospitalisationModel(SciensanoHospitalisationModel model)
        {
            Date = model.Date ?? new DateTime();
            Incoming = model.New_In;
            Outgoing = model.New_Out;
        }

        public DateTime Date { get; set; }
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
        public int PreviousTotalHospitalisations { get; set; }
        public int TotalHospitalisations => PreviousTotalHospitalisations + Incoming - Outgoing;
    }
}
