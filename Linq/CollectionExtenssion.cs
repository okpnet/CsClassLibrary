using System.Collections.Generic;
using System;

namespace LinqExtenssion
{
    /// <summary>
    /// LINQŠg’£ƒNƒ‰ƒX
    /// </summary>
    public static class CollectionExt
    {

        /// <summary>
        /// ”ÍˆÍ’Ç‰Á
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="addArray"></param>
        /// <returns></returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> array, ICollection<T> addArray)
        {
            var enumrator = addArray.GetEnumerator();
            while (enumrator.MoveNext())
            {
                var item = enumrator.Current;
                array.Add(item);
            }
            return array;
        }
        /// <summary>
        /// ”ÍˆÍ’Ç‰Á
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="addArray"></param>
        /// <returns></returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> array, IEnumerable<T> addArray)
        {
            var enumrator = addArray.GetEnumerator();
            while (enumrator.MoveNext())
            {
                var item = enumrator.Current;
                array.Add(item);
            }
            return array;
        }
    }
}
