using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Masterplan.Data;

namespace CompendiumImport.Tools
{

    internal static class EnumUtility
    {
        public static bool TryParse<T>(string value, out T result)
           where T : struct // error CS0702: Constraint cannot be special class 'System.Enum'
        {
            return TryParse<T>(value, out result, false);
        }

        //TODO : remove one of the functions
        //http://stackoverflow.com/questions/1082532/how-to-tryparse-for-enum-value/1082578#1082578
        public static bool TryParseEnum<T>(string str, bool caseSensitive, out T value) where T : struct
        {
            // Can't make this a type constraint...
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum");
            }
            var names = Enum.GetNames(typeof(T));
            value = (Enum.GetValues(typeof(T)) as T[])[0];  // For want of a better default
            foreach (var name in names)
            {
                if (String.Equals(name, str, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                {
                    value = (T)Enum.Parse(typeof(T), name);
                    return true;
                }
            }
            return false;
        }

        public static bool TryParse<T>(string value, out T result, bool ignoreCase)
           where T : struct // error CS0702: Constraint cannot be special class 'System.Enum'
        {
            result = default(T);
            try
            {
                result = (T)Enum.Parse(typeof(T), value, ignoreCase);
                return true;
            }
            catch { }

            return false;
        }

        public static string[] GetValue(Type enumToList)
        {
            Array a = Enum.GetValues(enumToList);
            string[] strArray = new string[a.Length];
            int i = 0;
            foreach (Object o in a)
            {
                strArray[i++] = o.ToString();
            }
            return strArray;
        }

    }

}
