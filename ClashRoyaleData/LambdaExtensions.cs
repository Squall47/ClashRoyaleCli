using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ClashRoyaleData
{
    public static class LambdaExtensions
    {
        public static Func<T, T, bool> CreateEqual<T, TMember>(Expression<Func<T, TMember>> selector)
        {
            var cible = Expression.Parameter(typeof(T), "cible");
            var client = Expression.Parameter(typeof(T), "target");
            var memberExpression = (MemberExpression)selector.Body;
            var property = (PropertyInfo)memberExpression.Member;
            var prop = Expression.Property(cible, property);
            var prop2 = Expression.Property(client, property);
            var equal = Expression.Equal(prop, prop2);
            return Expression.Lambda<Func<T, T, bool>>(equal, cible, client).Compile();
        }

        public static Func<T, string, bool> CreateEqualMember<T, TMember>(Expression<Func<T, TMember>> selector)
        {
            var cible = Expression.Parameter(typeof(T), "cible");
            var client = Expression.Parameter(typeof(string), "target");
            var memberExpression = (MemberExpression)selector.Body;
            var property = (PropertyInfo)memberExpression.Member;
            var prop = Expression.Property(cible, property);
            var equal = Expression.Equal(prop, client);
            return Expression.Lambda<Func<T,string, bool>>(equal, cible, client).Compile();
        }

    }
}