using Sciensano.CovidJson.Parser.Models;
using System.Collections.Generic;
using System.IO;

namespace Sciensano.CovidJson.Parser.ISciensanoParsers
{
    public interface ISciensanoParser
    {
        IList<ILocalModel> Parse(Stream stream);
    }
}
