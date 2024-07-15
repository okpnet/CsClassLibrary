using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringExtenssions
{
    /// <summary>
    /// 文字列変換
    /// </summary>
    public static class StringInflectionExtenssion
    {
        /// <summary>
        /// 単数形を複数形に変換
        /// https://qiita.com/KaoruHeart/items/0a6091108a47d335abbf
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Plural(this string value)
        {
            var plural = value;

            plural = Regex.Replace(plural, "s$", "s");
            plural = Regex.Replace(plural, "^(ax|test)is$", "$1es");
            plural = Regex.Replace(plural, "(octop|vir)us$", "$1i");
            plural = Regex.Replace(plural, "(octop|vir)i$", "$1i");
            plural = Regex.Replace(plural, "(alias|status)$", "$1es");
            plural = Regex.Replace(plural, "(bu)s$", "$1ses");
            plural = Regex.Replace(plural, "(buffal|tomat)o$", "$1oes");
            plural = Regex.Replace(plural, "([ti])um$", "$1a");
            plural = Regex.Replace(plural, "([ti])a$", "$1a");
            plural = Regex.Replace(plural, "sis$", "ses");
            plural = Regex.Replace(plural, "(?:([^f])fe|([lr])f)$", "$1$2ves");
            plural = Regex.Replace(plural, "(hive)$", "$1s");
            plural = Regex.Replace(plural, "([^aeiouy]|qu)y$", "$1ies");
            plural = Regex.Replace(plural, "(x|ch|ss|sh)$", "$1es");
            plural = Regex.Replace(plural, "(matr|vert|ind)(?:ix|ex)$", "$1ices");
            plural = Regex.Replace(plural, "^(m|l)ouse$", "$1ice");
            plural = Regex.Replace(plural, "^(m|l)ice$", "$1ice");
            plural = Regex.Replace(plural, "^(ox)$", "$1en");
            plural = Regex.Replace(plural, "^(oxen)$", "$1");
            plural = Regex.Replace(plural, "(quiz)$", "$1zes");

            plural = plural
                .Replace("person", "people")
                .Replace("man", "men")
                .Replace("child", "children")
                //.Replace("sex", "sexes")
                .Replace("move", "moves")
                .Replace("zombie", "zombies");

            if (plural == value) plural += "s";

            return plural;
        }

        public static string ToFormatStr(this string value,params object[] argments)
        {
            var result = string.Empty;
            try
            {
                result=string.Format(value, argments);
            }
            finally
            {
                result=result == null || result== "" ? value:result;
            }
            return result;
        } 
    }
}
