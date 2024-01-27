namespace Materia.Attributes;

public enum ScanType
{
    Default,
    Text,
    Static
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public abstract class MemberInjectionAttribute : Attribute
{
    public string? DetourName { get; init; }
    public bool EnableHook { get; init; } = true;
    public int Offset { get; init; }
    public bool Required { get; init; }
    public ScanType ScanType { get; init; }
    public int StaticOffset { get; init; }
}