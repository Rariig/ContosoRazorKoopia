using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contoso.Aids
{
    public static class CreateNew
    {
        public static T Instance<T>()
        {
            static T F()
            {
                var t = typeof(T);
                var o = Instance(t);

                return (o is null) ? default : (T)o;
            }
            var def = default(T);
            return Safe.Run(F, def);
        }
        public static object Instance(Type t)
        {
            return Safe.Run(() => {
                var constructor = getConstructorInfo(t);

                if (constructor is null) return null;
                var parameters = constructor.GetParameters();
                var values = setRandomParameters(parameters);
                return invoke(constructor, values);
            }, null);
        }
        private static object invoke(ConstructorInfo ci, object[] values)
        {
            return values.Length == 0 ? ci.Invoke(null) : ci.Invoke(values);
        }
        private static object[] setRandomParameters(IEnumerable<ParameterInfo> parameters)
        {
            var values = new List<object>();
            foreach (var p in parameters)
            {
                var t = p.ParameterType;
                var value = GetRandom.ValueOf(t);
                values.Add(value);
            }
            return values.ToArray();
        }
        private static ConstructorInfo getConstructorInfo(Type t)
        {
            var constructors = t?.GetConstructors();

            if (constructors == null) return null;
            return constructors.Length == 0 ? null : constructors[0];
        }
    }
}