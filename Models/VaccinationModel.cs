using Sciensano.CovidJson.Parser.SciensanoModels;
using System;

namespace Sciensano.CovidJson.Parser.Models
{
    public class VaccinationModel : ILocalModel
    {
        public VaccinationModel(SciensanoVaccinationModel model)
        {
            Date = model.Date ?? new DateTime();
            Count = model.Count;
            Brand = model.Brand;

            switch(model.Dose)
            {
                case SciensanoModels.Dose.A:
                    Dose = Dose.First;
                    break;
                case SciensanoModels.Dose.B:
                    Dose = Dose.Second;
                    break;
                case SciensanoModels.Dose.C:
                    Dose = Dose.OnlyDose;
                    break;
                case SciensanoModels.Dose.E:
                    Dose = Dose.Third;
                    break;
                case SciensanoModels.Dose.E2:
                    Dose = Dose.Fourth;
                    break;
                default:
                    Dose = Dose.Unknown;
                    break;
            }
        }

        public DateTime Date { get; set; }
        public int Count { get; set; }
        public Dose Dose { get; set; }
        public string Brand { get; set; }
    }

    public enum Dose
    {
        Unknown = 0,
        First,
        Second,
        Third,
        Fourth,
        OnlyDose
    }
}
