using ECGen.Generated;

namespace Materia.Game;

public static unsafe class UISystem
{
    public static UISystem_StaticFields* NativePtr { get; } = (UISystem_StaticFields*)GameInterop.GetClass<ECGen.Generated.UISystem>()->static_fields;
}