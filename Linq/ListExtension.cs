using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        /// <summary>
        /// The list moves a part of to a specified position.caution : index start is 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="insert"></param>
        /// <param name="replacements"></param>
        /// <exception cref="NotFiniteNumberException"></exception>
        public static void InsertReplace<T>(this IList<T> source,int insert,IEnumerable<T> replacements)
        {
            if( !replacements.Any(t => source.Contains(t)))
            {
                throw new NotFiniteNumberException($"No 'replacements' subobjects in source.");
            }

            var indexs = replacements.Select(source.IndexOf).OrderBy(t => t).ToArray();
            for(var index =0;indexs.Length>index;index++ )
            {
                InsertReplace(source, insert + index, indexs[index]);
            }
        }
        /// <summary>
        /// The list moves a part of to a specified position.caution : index start is 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="insert"></param>
        /// <param name="replace"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static void InsertReplace<T>(this IList<T> source, int insert,int replace)
        {
            if( insert == replace)
            {
                return;
            }

            if(0 > insert || insert > source.Count - 1)
            {
                throw new IndexOutOfRangeException($"'{insert}' argment is less 0 or greater than numer of list.");
            }
            if(0> replace || replace>source.Count - 1)
            {
                throw new IndexOutOfRangeException($"'{replace}' argment is less 0 or greater than numer of list.");
            }
            var range = replace > insert? replace - insert : insert - replace;
            var startIndex = replace > insert ? insert : replace + 1;
            var offset = replace > insert ? 1 : -1;
            var temps = new T[range];
            Array.Copy(source.ToArray(), startIndex, temps, 0, range);
            source[replace > insert ? insert : insert] = source[replace];
            for(var index = 0; range > index; index++)
            {
                source[startIndex + index + offset] = temps[index];
            }

        }
    }
}
