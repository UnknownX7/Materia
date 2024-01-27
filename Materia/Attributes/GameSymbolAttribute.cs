namespace Materia.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class GameSymbolAttribute : MemberInjectionAttribute
{
    public string Symbol { get; init; }
    public GameSymbolAttribute(string symbol) => Symbol = symbol;
}