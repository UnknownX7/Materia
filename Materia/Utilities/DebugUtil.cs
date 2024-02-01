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
            var klass = isClassType ? (Il2CppClass*)o : ((Il2CppObject*)o)->klass;
            var type = Util.ReadCString(klass->_1.name);
            var namespaze = Util.ReadCString(klass->_1.namespaze);
            return !string.IsNullOrEmpty(namespaze) ? $"{namespaze}.{type}" : type;
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
            var klass = isClassType ? (Il2CppClass*)o : ((Il2CppObject*)o)->klass;
            var fields = klass->_1.fields;
            for (int i = 0; i < klass->_2.field_count; i++)
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
                var klassField = typeof(T).GetField("klass");
                if (klassField == null) return;

                var klass = klassField.GetValue(*o);
                if (klass == null) return;

                var klassType = klassField.FieldType.GetElementType()!;
                var vtblField = klassType.GetField("vtable");
                if (vtblField == null) return;

                vtbl = vtblField.GetValue(Marshal.PtrToStructure((nint)Pointer.Unbox(klass), klassType));
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
}