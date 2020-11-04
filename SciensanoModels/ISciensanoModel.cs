using System;

namespace Sciensano.CovidJson.Parser.SciensanoModels
{
    public interface ISciensanoModel
    {
    }

    public interface IDatedSciensanoModel : ISciensanoModel
    {
        DateTime? Date { get; set; }
    }
}
