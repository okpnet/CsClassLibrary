using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringExtenssions
{
    /// <summary>
    /// フォーマット文字列拡張クラス
    /// </summary>
    public static class StringFormatExtession
    {
        /// <summary>
        /// フォーマット文字列オブジェクトから，{プロパティ名}に該当するプレースホルダを置換する
        /// プロパティ名が該当しない場合はプレースホルダを返す
        /// </summary>
        /// <param name="format">フォーマット</param>
        /// <param name="value">フォーマット文字列に割り当てるオブジェクト</param>
        /// <returns></returns>
        public static string? FormatString(this string format, object value)
        {
            if(format is (null or "") || value is null)
            {
                return "";
            }
            Dictionary<string, object?> ObjectToDictionary(object obj)
            {
                return obj.GetType()
                          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          .Where(prop => prop.CanRead)
                          .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null));
            }
            var propertyDictionary = ObjectToDictionary(value);
            return Regex.Replace(format, @"\{(\w+)\}", match =>
            {
                var key = match.Groups[1].Value;
                if (!propertyDictionary.ContainsKey(key))
                {
                    return key;
                }
                return propertyDictionary.TryGetValue(key, out var value) ? value?.ToString() : match.Value;
            });
        }
    }
}
