using DataCollection.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Extensions
{
    public static class EnumExtensions
    {
        public static int Value<T>(this T report) where T: Enum
        {
            return Convert.ToInt32(report);
        }

        public static string Name<T>(this T n) where T:Enum
        {
            return Enum.GetName(typeof(T), n);
        }

    }
}
