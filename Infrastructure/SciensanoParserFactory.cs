using Sciensano.CovidJson.Parser.SciensanoParsers;
using System;
using System.Linq;
using System.Reflection;

namespace Sciensano.CovidJson.Parser.Infrastructure
{
    public static class SciensanoParserFactory
    {
        public static ISciensanoParser GetParser(Type parserOf)
        {
            var type = Assembly
                .GetAssembly(typeof(ISciensanoParser))
                .GetTypes()
                .Where(t => t.IsClass &&
                            !t.IsAbstract &&
                            t.GetInterfaces().Contains(typeof(ISciensanoParser)) &&
                            (t.BaseType?.GenericTypeArguments?.Contains(parserOf) ?? false))
                .SingleOrDefault();

            if (type == null)
                throw new ArgumentException($"Unknown parser for type '{parserOf.Name}'");

            return Activator.CreateInstance(type) as ISciensanoParser;
        }
    }
}
