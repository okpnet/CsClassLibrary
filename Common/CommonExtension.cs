using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonExtensions
{
    public static class CommonExtension
    {
        /// <summary>
        /// キャストした値をOutにセットして､値を返す
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsDeclare<TResult>(this TResult value, out TResult result)
        {
            result = value;
            return result is not null;
        }
    }
}
