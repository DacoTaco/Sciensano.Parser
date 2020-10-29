using System;

namespace Sciensano.CovidJson.Parser.SciensanoModels
{
    interface ISciensanoModel
    {
    }

    interface IDatedSciensanoModel : ISciensanoModel
    {
        DateTime Date { get; set; }
    }
}
