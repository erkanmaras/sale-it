using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SaleIt.Infrastructure.Reflection
{
    public class TypeReflector
    {
        public bool TrySet<T>(object obj, string memberName, T value)
        {
            try
            {
                var mInfo = obj.GetType().GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

                switch (mInfo)
                {
                    case PropertyInfo pInfo when !pInfo.CanWrite:
                        return false;
                    case PropertyInfo pInfo:
                        pInfo.SetValue(obj, value);
                        break;
                    case FieldInfo fInfo:
                        fInfo.SetValue(obj, value);
                        break;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        [return: MaybeNull]
        public T TryGet<T>(object obj, string memberName)
        {
            try
            {
                var mInfo = obj.GetType().GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

                switch (mInfo)
                {
                    case PropertyInfo pInfo when !pInfo.CanWrite:
                        return default(T);
                    case PropertyInfo pInfo:
                        return (T)pInfo.GetValue(obj);
                    case FieldInfo fInfo:
                        return (T)fInfo.GetValue(obj);
                }
            }
            catch
            {
                // ignored
            }

            return default(T);
        }


        public static Func<TObject, TReturn> CreateGetter<TObject, TReturn>(object obj, string memberName)
        {

            var containerType = obj.GetType();
            var param = Expression.Parameter(typeof(TObject));

            Expression body = param;

            if (containerType != null && containerType != typeof(TObject))
            {
                body = Expression.Convert(param, containerType);
            }

            body = Expression.PropertyOrField(body, memberName);

            if (body.Type != typeof(TReturn))
            {
                body = Expression.Convert(body, typeof(TReturn));
            }

            return Expression.Lambda<Func<TObject, TReturn>>(body, param).Compile();
        }

        public static Action<TObject, TValue>? CreateSetter<TObject, TValue>(object obj, string memberName)
        {
            var type = obj.GetType();
            var objParam = Expression.Parameter(typeof(TObject));

            Expression body = objParam;

            if (type != null && type != typeof(TObject))
            {
                body = Expression.Convert(objParam, type);
            }

            body = Expression.PropertyOrField(body, memberName);

            var valueParam = Expression.Parameter(typeof(TValue));

            Expression value = valueParam;

            if (body.Type != typeof(TValue))
            {
                value = Expression.Convert(valueParam, body.Type);
            }

            return Expression.Lambda<Action<TObject, TValue>>(Expression.Assign(body, value), objParam, valueParam).Compile();
        }
    }
}
