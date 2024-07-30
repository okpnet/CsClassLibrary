using System.Linq;

namespace StringExtenssions
{
    public static partial class StringPascalSnakeExt
    {
        [System.Text.RegularExpressions.GeneratedRegex("_")]
        private static partial System.Text.RegularExpressions.Regex SnakeCaseRegex();
        /// <summary>
        /// スネークケースをパスカルケースに変換
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPascalFromSnake(this string str)
        {
            if (str is (null or "")) return str??string.Empty;

            var splits = SnakeCaseRegex().Split(str);
            var result = splits.Aggregate((a, b) =>
            {
                var value = "";
                if (a.Length >= 1)
                {
                    value += a[0].ToString().ToUpper() + a.Substring(1);
                }
                else
                {
                    value += a;
                }
                if (b.Length >= 1)
                {
                    value += b[0].ToString().ToUpper() + b.Substring(1);
                }
                return value;
            });
            return result;
        }


    }
}
