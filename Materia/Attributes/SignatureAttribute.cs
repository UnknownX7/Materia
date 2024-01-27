namespace Materia.Attributes;

public enum ScannerType
{
    Exe,
    GameAssembly,
    NtDll,
    UnityPlayer
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SignatureAttribute : MemberInjectionAttribute
{
    public string Signature { get; init; }
    public ScannerType Scanner { get; init; } = ScannerType.GameAssembly;
    public string? CustomScanner { get; init; }
    public SignatureAttribute(string signature) => Signature = signature;
}