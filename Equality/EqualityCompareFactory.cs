
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace TreeViewLib.Extensions
{
    public static class EqulityCompareFactory
    {
        /// <summary>
        /// T型のオブジェクトを任意の方法で比較するイコールコンペア生成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pridicate"></param>
        /// <returns></returns>
        public static IEqualityComparer<T> Create<T>(Func<T,T,bool> pridicate)
        {
            return new EqualityComparesImmediately<T>(pridicate);
        }
        /// <summary>
        /// T型のオブジェクトの特定のロパティを比較するイコールコンペア生成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IEqualityComparer<T> Create<T>(Expression<Func<T,object>> expression)
        {
            return new EqualityCompareProperty<T>(expression.Compile());
        }
    }
    /// <summary>
    /// T型を任意の比較をするイコールコンペアクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualityComparesImmediately<T> : IEqualityComparer<T>
    {
        readonly Func<T,int> _hashs;
        readonly Func<T, T, bool> _pridicate;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pridicate">比較式</param>
        public EqualityComparesImmediately(Func<T, T, bool> pridicate):this(pridicate,x=>x.GetHashCode())
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pridicate">比較式</param>
        /// <param name="hashs">ハッシュ</param>
        public EqualityComparesImmediately(Func<T,T,bool> pridicate,Func<T,int> hashs)
        {
            _hashs = hashs;
            _pridicate = pridicate;
        }

        public bool Equals(T? x, T? y)
        {
            return _pridicate(x, y);
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return _hashs(obj);
        }
    }
    /// <summary>
    /// T型のプロパティを比較するイコールコンペアクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualityCompareProperty<T>:IEqualityComparer<T>
    {
        readonly Func<T, object> _propertyPridicate;
        readonly Func<T, int> _hashs;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyPridicate">プロパティ</param>
        /// <param name="hashs">ハッシュ</param>
        public EqualityCompareProperty(Func<T, object> propertyPridicate, Func<T, int> hashs) 
        {
            _hashs = hashs;
            _propertyPridicate = propertyPridicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyPridicate">プロパティ</param>
        public EqualityCompareProperty(Func<T, object> propertyPridicate):this(propertyPridicate,x=>x.GetHashCode())
        {
            _propertyPridicate = propertyPridicate;
        }

        public bool Equals(T? x, T? y)
        {
            if (x is null && y is null) return true;
            if(x is null || y is null)return false;
            var xval=_propertyPridicate.Invoke(x);
            var yval=_propertyPridicate.Invoke(y);
            return xval.Equals(yval);
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode();
        }

    }

}