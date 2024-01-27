using Materia.Plugin;

namespace Materia;

internal class MateriaConfig : PluginConfiguration
{
    public float UIScale { get; set; } = 1;
    public Logging.LogType LoggingLevel { get; set; } = Logging.LogType.Info;
}