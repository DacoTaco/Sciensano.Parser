using System;
using System.Linq;
using System.Reflection;

namespace Sciensano.CovidJson.Parser.ISciensanoParsers
{
    public static class SciensanoParserSelector
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
                .Single();
            return Activator.CreateInstance(type) as ISciensanoParser;
        }
    }
}
