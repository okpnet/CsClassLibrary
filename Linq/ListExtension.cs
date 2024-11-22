using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListExtensions
{
    public static class ListExtension
    {
        /// <summary>
        /// The list item replacement of argment 'original' and 'insert'.caution : index start is 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="original"></param>
        /// <param name="insert"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static void Replace<T>(this IList<T> source,int original,int insert)
        {
            if(source.Count == 0) 
            {
                return;
            }
            if(0 > original || original>source.Count )
            {
                throw new IndexOutOfRangeException($"original '{original}' argment is less 0 or greater than numer of list.");
            }
            if (0 > insert || insert > source.Count)
            {
                throw new IndexOutOfRangeException($"insert '{insert}' argment is less 0 or greater than numer of list.");
            }
            var temp = source[insert];
            source[insert] = source[original];
            source[original] = temp;
        }
        /// <summary>
        /// The list item replacement of argument array 'original' and 'insert' pair.caution : index start is 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="originalAndInserts"></param>
        public static void Replace<T>(this IList<T> source,params (int,int)[] originalAndInserts)
        {
            if (originalAndInserts.Length == 0 || source.Count==0)
            {
                return;
            }
            foreach(var indexs in originalAndInserts)
            {
                Replace(source, indexs.Item1, indexs.Item2);
            }
        }
    }
}
