
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace TreeViewLib.Extensions
{
    public static class EqulityCompareFactory
    {
        /// <summary>
        /// T�^�̃I�u�W�F�N�g��C�ӂ̕��@�Ŕ�r����C�R�[���R���y�A����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pridicate"></param>
        /// <returns></returns>
        public static IEqualityComparer<T> Create<T>(Func<T,T,bool> pridicate)
        {
            return new EqualityComparesImmediately<T>(pridicate);
        }
        /// <summary>
        /// T�^�̃I�u�W�F�N�g�̓���̃��p�e�B���r����C�R�[���R���y�A����
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
    /// T�^��C�ӂ̔�r������C�R�[���R���y�A�N���X
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualityComparesImmediately<T> : IEqualityComparer<T>
    {
        readonly Func<T,int> _hashs;
        readonly Func<T, T, bool> _pridicate;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pridicate">��r��</param>
        public EqualityComparesImmediately(Func<T, T, bool> pridicate):this(pridicate,x=>x.GetHashCode())
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pridicate">��r��</param>
        /// <param name="hashs">�n�b�V��</param>
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
    /// T�^�̃v���p�e�B���r����C�R�[���R���y�A�N���X
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualityCompareProperty<T>:IEqualityComparer<T>
    {
        readonly Func<T, object> _propertyPridicate;
        readonly Func<T, int> _hashs;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyPridicate">�v���p�e�B</param>
        /// <param name="hashs">�n�b�V��</param>
        public EqualityCompareProperty(Func<T, object> propertyPridicate, Func<T, int> hashs) 
        {
            _hashs = hashs;
            _propertyPridicate = propertyPridicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyPridicate">�v���p�e�B</param>
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