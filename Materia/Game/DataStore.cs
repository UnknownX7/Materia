namespace Materia.Game;

public static unsafe class DataStore
{
    private static readonly ECGen.Generated.Command.DB.DataStore.StaticFields* staticFields = (ECGen.Generated.Command.DB.DataStore.StaticFields*)Il2CppType<ECGen.Generated.Command.DB.DataStore>.Instance.NativePtr->staticFields;
    public static ECGen.Generated.Command.DB.DataStore* NativePtr => staticFields->instance;
}