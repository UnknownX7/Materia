using System.Numerics;
using ImGuiNET;
using Materia.Plugin;

namespace Materia.UI;

internal static class PluginMenu
{
    private static bool lastHovered = false;
    private static bool displayDemo = false;

    public static void Draw(PluginManager pluginManager)
    {
        ImGui.PushStyleVar(ImGuiStyleVar.Alpha, lastHovered ? 1 : 0.001f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(25 * ImGuiEx.Scale));
        ImGui.SetNextWindowPos(Vector2.Zero, ImGuiCond.Always);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, 0xA0007F00);
        ImGui.Begin("PluginMenuButton", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration);
        ImGui.PopStyleColor();
        lastHovered = ImGui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows | ImGuiHoveredFlags.AllowWhenBlockedByPopup | ImGuiHoveredFlags.AllowWhenBlockedByActiveItem);
        if (ImGui.BeginPopupContextWindow("Plugins", ImGuiPopupFlags.MouseButtonLeft))
        {
            lastHovered = true;

            pluginManager.InvokeAll(p =>
            {
                if (p.PluginServiceManager?.EventHandler.ToggleMenu != null && ImGui.Selectable(p.PluginName, false))
                    p.PluginServiceManager?.EventHandler.ToggleMenu();
            }, nameof(PluginEventHandler.ToggleMenu));

            if (ImGui.MenuItem("ImGui Demo"))
                displayDemo ^= true;

            if (ImGui.MenuItem($"{nameof(Materia)} Settings"))
                SettingsMenu.IsVisible ^= true;
        }
        ImGui.End();
        ImGui.PopStyleVar(2);

        if (displayDemo)
            ImGui.ShowDemoWindow(ref displayDemo);
    }
}