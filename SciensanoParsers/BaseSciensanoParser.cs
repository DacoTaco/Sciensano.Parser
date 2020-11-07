using Newtonsoft.Json;
using Sciensano.CovidJson.Parser.Models;
using Sciensano.CovidJson.Parser.SciensanoModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sciensano.CovidJson.Parser.SciensanoParsers
{
    public interface ISciensanoParser
    {
        IList<ILocalModel> Parse(Stream stream);
    }

    public abstract class BaseSciensanoParser<T,Y> : ISciensanoParser 
        where T : ILocalModel 
        where Y : ISciensanoModel
    {
        public BaseSciensanoParser() { }

        public abstract IList<ILocalModel> Parse(Stream stream);

        protected static IEnumerable<Y> ParseJson(Stream stream)
        {
            if ((stream?.Length ?? 0) <= 0)
                throw new Exception("Invalid stream to convert data from");

            var list = new List<Y>();
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    // deserialize only when there's "{" character in the stream
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var _object = serializer.Deserialize<Y>(reader);
                        list.Add(_object);
                    }
                }
            }

            if (typeof(Y).GetInterfaces().Contains(typeof(IDatedSciensanoModel)))
            {
                list = list.OrderBy(x => ((IDatedSciensanoModel)x).Date).ToList();
            }

            return list.AsEnumerable();
        }
    }
}
