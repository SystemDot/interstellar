namespace Interstellar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static IEnumerable<Type> FromSameAssemblyWhereImplements<T>(this Type type)
        {
            return type.GetTypesInAssembly().WhereNormalConcrete().WhereImplements<T>();
        }

        public static IEnumerable<Type> GetTypesInAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly.ExportedTypes;
        }

        private static IEnumerable<Type> WhereNormalConcrete(this IEnumerable<Type> types) => types.WhereNonAbstract().WhereNonGeneric().WhereConcrete();

        private static IEnumerable<Type> WhereNonAbstract(this IEnumerable<Type> types) => types
            .Where(t => !t.GetTypeInfo().IsAbstract);

        private static IEnumerable<Type> WhereConcrete(this IEnumerable<Type> types) => types
            .Where(t => !t.GetTypeInfo().IsInterface && t.GetTypeInfo().IsClass);

        private static IEnumerable<Type> WhereNonGeneric(this IEnumerable<Type> types) =>
            types.Where(t => !t.GetTypeInfo().ContainsGenericParameters);

        public static IEnumerable<Type> WhereImplements<TImplemented>(this IEnumerable<Type> types)
        {
            return types
                .Where(t => t.GetNonBaseInterfaces().Contains(typeof(TImplemented))
                    || t.GetBaseInterfaces().Contains(typeof(TImplemented)));
        }

        private static IEnumerable<Type> GetNonBaseInterfaces(this Type type)
        {
            IEnumerable<Type> baseInterfaces = type.GetBaseInterfaces();
            return type.GetInterfaces().Where(t => !baseInterfaces.Contains(t));
        }

        private static IEnumerable<Type> GetBaseInterfaces(this Type type)
        {
            var baseInterfaces = new List<Type>();
            Type baseType = type.GetTypeInfo().BaseType;
            
            if (baseType == typeof(MemberInfo))
            {
                return baseInterfaces;
            }

            while (baseType != null)
            {
                baseInterfaces.AddRange(baseType.GetInterfaces());
                baseType = baseType.GetTypeInfo().BaseType;
                if (baseType == typeof(MemberInfo))
                {
                    return baseInterfaces;
                }
            }

            return baseInterfaces;
        }
    }
}
