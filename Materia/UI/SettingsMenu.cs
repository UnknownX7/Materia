using System.Diagnostics;
using System.Numerics;
using ImGuiNET;

namespace Materia.UI;

internal static class SettingsMenu
{
    private static bool isVisible;
    public static ref bool IsVisible => ref isVisible;

    private static float scale = ImGuiEx.Scale;

    public static void Draw()
    {
        if (!isVisible) return;

        ImGui.SetNextWindowSizeConstraints(new Vector2(400, 300) * ImGuiEx.Scale, new Vector2(10000));
        if (!ImGui.Begin($"{nameof(Materia)} Settings", ref isVisible))
        {
            ImGui.End();
            return;
        }

        if (ImGui.BeginTabBar("SettingsTabs"))
        {
            if (ImGui.BeginTabItem("General"))
            {
                if (ImGui.Button("Apply"))
                    ImGuiEx.Scale = scale;
                ImGui.SameLine();
                ImGui.SliderFloat("UI Scale", ref scale, 0.5f, 2, "%.2f", ImGuiSliderFlags.AlwaysClamp);
                ImGui.EndTabItem();
            }

            // TODO: Actually make a console
            if (ImGui.BeginTabItem("Console"))
            {
                if (ImGui.SmallButton("Open Log"))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = Logging.MainLogFilePath });
                    }
                    catch { }
                }

                ImGui.SameLine();

                if (ImGui.SmallButton("Clear"))
                {
                    lock (Logging.MainLogOutputLines)
                        Logging.MainLogOutputLines.Clear();
                }

                ImGui.BeginChild("ConsoleLines", Vector2.Zero, false, ImGuiWindowFlags.HorizontalScrollbar);
                for (int i = 0; i < Logging.MainLogOutputLines.Count; i++)
                    ImGui.TextUnformatted(Logging.MainLogOutputLines[i]);
                ImGui.EndChild();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        ImGui.End();
    }
}