namespace Materia.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class GameSymbolAttribute : MemberInjectionAttribute
{
    public string Symbol { get; init; }
    public bool ReturnPointer { get; set; }
    public GameSymbolAttribute(string symbol) => Symbol = symbol;
}