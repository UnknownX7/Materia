using ECGen.Generated;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Materia.Utilities;

public static unsafe class DebugUtil
{
    public static void PrintObject(object o)
    {
        foreach (var member in o.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public).Where(member => member.MemberType is MemberTypes.Field or MemberTypes.Property))
            Console.WriteLine($"{member.Name} = {member.GetValue(o)}");
    }

    public static string GetTypeName(void* o, bool isClassType = false)
    {
        if (o == null) return string.Empty;

        try
        {
            var @class = isClassType ? (Il2CppClass*)o : ((Il2CppObject*)o)->@class;
            var type = Util.ReadCString(@class->name);
            var @namespace = Util.ReadCString(@class->@namespace);
            return !string.IsNullOrEmpty(@namespace) ? $"{@namespace}.{type}" : type;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static void PrintType(void* o, bool isClassType = false) => Console.WriteLine(GetTypeName(o, isClassType));

    public static void PrintFields(void* o, bool isClassType = false) // TODO: Inherited fields
    {
        if (o == null) return;

        try
        {
            var @class = isClassType ? (Il2CppClass*)o : ((Il2CppObject*)o)->@class;
            var fields = @class->fields;
            for (int i = 0; i < @class->fieldCount; i++)
            {
                var field = fields[i];
                Console.WriteLine($"[{field.offset:X}] {((nint)field.name).ReadCString()}");
            }
        }
        catch { }
    }

    public static void PrintVirtualFunctions<T>(T* o, bool isClassType = false) where T : unmanaged
    {
        if (o == null) return;

        try
        {
            object? vtbl;
            if (isClassType)
            {
                var vtblField = typeof(T).GetField("vtable");
                if (vtblField == null) return;

                vtbl = vtblField.GetValue(*o);
            }
            else
            {
                var classField = typeof(T).GetField("@class");
                if (classField == null) return;

                var @class = classField.GetValue(*o);
                if (@class == null) return;

                var classType = classField.FieldType.GetElementType()!;
                var vtblField = classType.GetField("vtable");
                if (vtblField == null) return;

                vtbl = vtblField.GetValue(Marshal.PtrToStructure((nint)Pointer.Unbox(@class), classType));
            }

            if (vtbl == null) return;

            foreach (var fieldInfo in vtbl.GetType().GetAllFields())
            {
                var fieldObj = fieldInfo.GetValue(vtbl);
                if (fieldObj == null) continue;

                var virtualInvokeData = (Il2CppVirtualInvokeData)fieldObj;
                var ptr = virtualInvokeData.method->name;
                if (ptr == null) continue;

                var vfName = Util.ReadCString(ptr);
                Console.WriteLine($"[{virtualInvokeData.methodPtr:X}] {fieldInfo.Name} = {vfName}");
            }
        }
        catch { }
    }

    public static object Print(object o)
    {
        Console.WriteLine(o);
        return o;
    }

    public static T* Print<T>(T* ptr) where T : unmanaged
    {
        Console.WriteLine($"{(nint)ptr:X}");
        return ptr;
    }
}