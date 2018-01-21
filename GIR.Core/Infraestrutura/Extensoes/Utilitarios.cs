using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GIR.Core.Infraestrutura.Extensoes
{
    public static class Utilitarios
    {
        public static string GetDescription(this Enum enumVal)
        {
            var type = enumVal.GetType();
            var attribute = type.GetMember(enumVal.ToString())[0]
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .SingleOrDefault();

            var description = (attribute != null) ? attribute.Description : string.Empty;

            return description;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }
}
