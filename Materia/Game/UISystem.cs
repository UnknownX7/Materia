namespace Materia.Game;

public static unsafe class UISystem
{
    public static ECGen.Generated.UISystem.StaticFields* NativePtr { get; } = (ECGen.Generated.UISystem.StaticFields*)GameInterop.GetClass<ECGen.Generated.UISystem>()->static_fields;
}