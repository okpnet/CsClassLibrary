using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtenssions
{
    /// <summary>
    /// Enumerable拡張クラス
    /// </summary>
    public static class EnumrableExtenssion
    {
        /// <summary>
        /// IEnumerableを実体化してループする
        /// </summary>
        /// <typeparam name="T">IEnumerableが保持している型</typeparam>
        /// <param name="source">IEnumerable</param>
        /// <param name="action">ループ処理のデリゲート</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source,Action<T> action)
        {
            foreach(var item in source)action(item);
            return source;
        }
    }
}
