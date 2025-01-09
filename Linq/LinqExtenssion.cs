using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtenssions
{
    public static class LinqExtenssion
    {
        /// <summary>
        /// 重複リストを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TKey> FindDuplication<T,TKey>(this IEnumerable<T> source, Func<T, TKey> predicate)
        {
            return source.GroupBy(predicate).Where(t=>t.Count()>1).Select(t=>t.Key);
        }
        /// <summary>
        /// NULL除外
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
        {
            if (source == null) return Enumerable.Empty<T>();
            return source.Where(x => x != null)!;
        }
        /// <summary>
        /// IEnumerableを実体化してループする
        /// </summary>
        /// <typeparam name="T">IEnumerableが保持している型</typeparam>
        /// <param name="source">IEnumerable</param>
        /// <param name="action">ループ処理のデリゲート</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
            return source;
        }
        /// <summary>
        /// 祖先の平坦化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">ソース</param>
        /// <param name="self">探索するオブジェクト</param>
        /// <param name="getParent">第2引数の親を第1引数ソースから探して、親を返す</param>
        /// <returns>平坦化した祖先のリスト</returns>
        public static IEnumerable<T> GetAncester<T>(IEnumerable<T> list, T? self, Func<IEnumerable<T>, T, T?> getParent)
        {
            if (self is null)
            {
                yield break;
            }
            yield return self;
            foreach (var result in GetAncester(list, getParent(list, self), getParent))
            {
                yield return result;
            }
        }
    }
}
