using System.Reflection;
using System.Runtime.InteropServices;

namespace Materia.Utilities;

public static class Extensions
{
    public static string ToHexString(this IEnumerable<byte> data) => string.Join(" ", data.Select(b => b.ToString("X2")));

    public static string SnakeCaseToPascalCase(this string s) => s
        .Split('_', StringSplitOptions.RemoveEmptyEntries)
        .Select(w => char.ToUpperInvariant(w[0]) + w[1..])
        .Aggregate(string.Empty, (s1, s2) => s1 + s2);

    public static IEnumerable<Type> GetTypes<T>(this Assembly assembly) => typeof(T).IsInterface
    ? assembly.GetTypes().Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(T)))
    : assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(T)));

    public static IEnumerable<(Type, T)> GetTypesWithAttribute<T>(this Assembly assembly) where T : Attribute =>
        from t in assembly.GetTypes() let attribute = t.GetCustomAttribute<T>() where attribute != null select (t, attribute);

    public static MemberInfo[] GetAllMembers(this IReflect type) => type.GetMembers(Util.AllMembersBindingFlags);

    public static IEnumerable<(MemberInfo, T)> GetAllMembersWithAttribute<T>(this IReflect type) where T : Attribute =>
        from memberInfo in type.GetAllMembers() let attribute = memberInfo.GetCustomAttribute<T>() where attribute != null select (memberInfo, attribute);

    public static FieldInfo[] GetAllFields(this IReflect type) => type.GetFields(Util.AllMembersBindingFlags);

    public static PropertyInfo[] GetAllProperties(this IReflect type) => type.GetProperties(Util.AllMembersBindingFlags);

    public static MethodInfo[] GetAllMethods(this IReflect type) => type.GetMethods(Util.AllMembersBindingFlags);

    public static bool DeclaresMethod(this Type type, string method, Type[] types) => type.GetMethod(method, Util.AllMembersBindingFlags, types)?.DeclaringType == type;

    public static bool DeclaresMethod(this Type type, string method) => type.DeclaresMethod(method, Type.EmptyTypes);

    public static Type? GetObjectType(this MemberInfo memberInfo) => memberInfo switch
    {
        FieldInfo field => field.FieldType,
        PropertyInfo property => property.PropertyType,
        _ => null
    };

    public static object? GetValue(this MemberInfo memberInfo, object? o) => memberInfo switch
    {
        FieldInfo field => field.GetValue(o),
        PropertyInfo property => property.GetValue(o),
        _ => null
    };

    public static void SetValue(this MemberInfo memberInfo, object? o, object? value)
    {
        switch (memberInfo)
        {
            case FieldInfo field:
                field.SetValue(o, value);
                break;
            case PropertyInfo property:
                property.SetValue(o, value);
                break;
        }
    }

    public static string ReadCString(this nint address) => Marshal.PtrToStringAnsi(address)!;
    public static string ReadCString(this nint address, int len) => Marshal.PtrToStringAnsi(address, len);
}