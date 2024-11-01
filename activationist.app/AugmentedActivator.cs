using System.Reflection;

namespace activationist.app;

/// <summary>
/// Factory of objects of the specified type that uses the constructor with the highest number of arguments
/// resolved from the dependency injection container and augmented with a set of pre-created objects.
/// </summary>
public class AugmentedActivator<T>(IServiceProvider serviceProvider) where T : class
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Type Type = typeof(T);

    // ReSharper disable once StaticMemberInGenericType
    private static readonly Constructor[] Constructors = GetConstructors();

    private readonly Dictionary<Type, object> _dependencies = new();

    public void AddDependency<TDependency>(TDependency dependency) where TDependency : class
    {
        _dependencies.Add(typeof(TDependency), dependency);
    }

    public T? Create()
    {
        Dictionary<Type, object> cache = new(_dependencies);

        foreach (var constructor in Constructors)
        {
            if (!constructor.IsPublic)
            {
                continue;
            }

            var parameterValues = new object[constructor.Parameters.Length];
            int index = 0;
            bool gotAll = true;

            foreach (var info in constructor.Parameters)
            {
                var value = GetOrResolve(info.ParameterType, cache);

                if (value is null)
                {
                    gotAll = false;
                    break;
                }

                parameterValues[index++] = value;
            }

            if (gotAll)
            {
                var created = Activator.CreateInstance(Type, parameterValues);

                if (created is not null)
                {
                    return created as T;
                }
            }
        }

        return null;
    }

    private static Constructor[] GetConstructors()
    {
        var constructors = typeof(T).GetConstructors();
        Constructor[] result = new Constructor[constructors.Length];
        int i = 0;
        foreach (var constructor in constructors)
        {
            result[i++] = new Constructor(constructor);
        }

        Array.Sort(result, ArgumentNumberComparer.Comparer);
        return result;
    }

    private object? GetOrResolve(Type type, Dictionary<Type, object> cache)
    {
        if (!cache.TryGetValue(type, out object? value))
        {
            value = serviceProvider.GetService(type);
            if (value is not null)
            {
                cache.Add(type, value);
            }
        }

        return value;
    }

    private struct Constructor(ConstructorInfo info)
    {
        public readonly bool IsPublic = info.IsPublic;
        public readonly ParameterInfo[] Parameters = info.GetParameters();
    }

    private sealed class ArgumentNumberComparer : IComparer<Constructor>
    {
        public static readonly IComparer<Constructor> Comparer = new ArgumentNumberComparer();

        int IComparer<Constructor>.Compare(Constructor x, Constructor y)
        {
            int countX = x.Parameters.Length;
            int countY = y.Parameters.Length;

            return countX < countY ? 1 : countX == countY ? 0 : -1;
        }
    }
}