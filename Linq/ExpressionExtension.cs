using System.Linq.Expressions;
using System.Reflection;

namespace LinqExtenssions
{
    /// <summary>
    /// Expressionファクトリ拡張クラス
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// ラムダ式からパス取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetPropertyPathStr<T>(Expression<Func<T, object>> member) where T : class
        {
            var expression = member.Body switch
            {
                MemberExpression m => m,
                UnaryExpression u => u.Operand,
                _ => null
            };

            if (expression == null) return "";

            var stack = new Stack<string>();

            while (expression is MemberExpression memberExpr)
            {
                stack.Push(memberExpr.Member.Name);
                expression = memberExpr.Expression!;
            }

            return string.Join(".", stack);
        }
        /// <summary>
        /// メンバーパスからプロパティインフォ取得
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        public static PropertyInfo? GetPropertyInfoFromPathStr(this Type type, string propertyPath)
        {
            var param = Expression.Parameter(type);
            Expression body = type.GetExpressionFieldFromType(propertyPath,param);

            var lambda = Expression.Lambda(body, param);
            MemberExpression memberex;
            if (lambda.Body is UnaryExpression unary && unary != null)
            {
                memberex = (MemberExpression)unary.Operand;
            }
            else
            {
                memberex = (MemberExpression)lambda.Body;
            }

            var propInf = (PropertyInfo)memberex.Member;
            return propInf;
        }
        /// <summary>
        /// タイプとパスからフィールド式を取得
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        public static Expression GetExpressionFieldFromType(this Type type, string propertyPath,ParameterExpression param)
        {
            //var param = Expression.Parameter(type,"_arg_"+type.Name);
            Expression body = param;
            foreach (var member in propertyPath.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            };
            return body;
        }
        /// <summary>
        /// インスタンスとパスから、子プロパティを含めて検索してオブジェクトを取り出す
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        public static object? GetValueFromPropertyPath<T>(this T instance, string propertyPath) where T:class
        {
            object? value = instance;
            foreach (var propName in propertyPath.Split('.'))
            {
                if (value is null) return null;
                var info = value.GetType().GetProperty(propName);
                value = info?.GetValue(value, null);
            }
            return value;
        }
        /// <summary>
        /// オブジェクトから値の取得
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Tval? GetValue(Expression<Func<Tval>> expression)
        {
            if (expression.Body is not MemberExpression member)
            {
                throw new InvalidOperationException("No support member");
            }
            var inst = GetInstanceHelper(member);
            var exp = member.Member;
            return exp switch
            {
                FieldInfo field => (Tval?)field.GetValue(inst),
                PropertyInfo property => (Tval?)property.GetValue(inst),
                _ => throw new InvalidOperationException("No support member")
            };
        }
        /// <summary>
        /// オブジェクトへ値のセット
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetValue(Expression<Func<Tval>> expression, Tval value)
        {
            if (expression.Body is not MemberExpression member)
            {
                throw new InvalidOperationException("Not supported.");
            }
            var inst = GetInstanceHelper(member);
            var exp = member.Member;
            switch (exp)
            {
                case FieldInfo field:
                    field.SetValue(inst, value);
                    break;
                case PropertyInfo property:
                    property.SetValue(inst, value);
                    break;
                default:
                    throw new InvalidOperationException("No support member");
            }
        }
        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public static object? GetInstanceHelper(MemberExpression memberExpression)
        {
            return memberExpression.Expression switch
            {
                ConstantExpression constantExpression => constantExpression.Value,
                MemberExpression member => GetInstanceHelper(member),
                _ => null
            };
        }
        public static Func<TOwner, TOptionType> BuildGetter<TOwner, TOptionType>(Expression<Func<TOwner, TOptionType>> propertyExpression)
        {
            return propertyExpression.Compile();
        }

        public static Action<TOwner, TOptionType> BuildSetter<TOwner, TOptionType>(Expression<Func<TOwner, TOptionType>> propertyExpression)
        {
            if (propertyExpression.Body is not MemberExpression memberExpr)
                throw new ArgumentException("Expression must be a MemberExpression", nameof(propertyExpression));

            // 右辺: value（代入する値）
            var valueParam = Expression.Parameter(typeof(TOptionType), "value");

            // 左辺の最も深いプロパティ（例: Baz）を探す
            var memberStack = new Stack<MemberExpression>();
            Expression? current = memberExpr;

            while (current is MemberExpression m)
            {
                memberStack.Push(m);
                current = m.Expression;
            }

            if (memberStack.Count == 0)
                throw new ArgumentException("No valid member expression found", nameof(propertyExpression));

            // 所有者（TOwner）のパラメータ（例: x）
            var ownerParam = propertyExpression.Parameters[0];

            // 左辺の構築（ネストされたオブジェクトのプロパティアクセスを構築）
            Expression? targetExpr = ownerParam;
            while (memberStack.Count > 1) // 最後は setter の対象なので飛ばす
            {
                var m = memberStack.Pop();
                targetExpr = Expression.MakeMemberAccess(targetExpr, m.Member);
            }

            var finalMember = memberStack.Pop();
            if (finalMember.Member is not PropertyInfo finalProp)
                throw new ArgumentException("The final member is not a property.", nameof(propertyExpression));

            var setterMethod = finalProp.GetSetMethod(nonPublic: false);
            if (setterMethod == null)
                throw new InvalidOperationException($"Property '{finalProp.Name}' does not have a public setter.");

            // 左辺: targetExpr.FinalProperty = value
            var assignExpr = Expression.Assign(
                Expression.Property(targetExpr!, finalProp),
                valueParam
            );

            var lambda = Expression.Lambda<Action<TOwner, TOptionType>>(assignExpr, ownerParam, valueParam);
            return lambda.Compile();
        }
    }
}
