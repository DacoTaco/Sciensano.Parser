using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sciensano.CovidJson.Parser.SciensanoParsers
{
    public interface ISciensanoParser
    {
        Task<List<ILocalModel>> ParseAsync(Stream stream);
    }

    public abstract class BaseSciensanoParser<TLocalModel,YSciensanoModel> : ISciensanoParser 
        where TLocalModel : ILocalModel 
        where YSciensanoModel : ISciensanoModel
    {
        public BaseSciensanoParser() { }

        public abstract Task<List<ILocalModel>> ParseAsync(Stream stream);

        protected static IEnumerable<YSciensanoModel> ParseJson(Stream stream)
        {
            if ((stream?.Length ?? 0) <= 0)
                throw new Exception("Invalid stream to convert data from");
         
            using var jsonDocument = JsonDocument.Parse(stream);
            if (jsonDocument?.RootElement.ValueKind != JsonValueKind.Array)
                return Enumerable.Empty<YSciensanoModel>();

            var list = new List<YSciensanoModel>();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = 
                {
                    new JsonStringEnumConverter()
                }
            };
            foreach (var element in jsonDocument.RootElement.EnumerateArray())
                list.Add(element.Deserialize<YSciensanoModel>(options));

            if (typeof(YSciensanoModel).GetInterfaces().Contains(typeof(IDatedSciensanoModel)))
            {
                list = list.OrderBy(x => ((IDatedSciensanoModel)x).Date).ToList();
            }

            return list.AsEnumerable();
        }
    }
}
