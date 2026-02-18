using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace AntiFun;

public class AntiFun : BasePlugin
{
    public override string ModuleName => "Anti-Fun";
    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "jvnipers";
    public override string ModuleDescription => "Breaks all func_breakable entities on command";

    public override void Load(bool hotReload)
    {
        AddCommand("css_breakall", "Break all func_breakable entities", Cmd_BreakAll);
        AddCommand("css_breakall_debug", "Break all func_breakable entities with debug info", Cmd_BreakAllDebug);
    }
    private int BreakAllBreakables()
    {
        int count = 0;

        var breakables = Utilities.FindAllEntitiesByDesignerName<CBreakable>("func_breakable");
        foreach (var entity in breakables)
        {
            if (entity == null || !entity.IsValid) continue;

            entity.AcceptInput("Break");
            count++;
        }

        return count;
    }

    private void Cmd_BreakAll(CCSPlayerController? player, CommandInfo command)
    {
        int count = BreakAllBreakables();

        if (player != null && player.IsValid)
        {
            player.PrintToChat($"No more fun. Broke {count} func_breakable entities.");
        }
        else
        {
            Server.PrintToConsole($"No more fun. Broke {count} func_breakable entities.");
        }
    }

    private void Cmd_BreakAllDebug(CCSPlayerController? player, CommandInfo command)
    {
        int count = 0;

        var breakables = Utilities.FindAllEntitiesByDesignerName<CBreakable>("func_breakable");

        if (player != null && player.IsValid)
        {
            player.PrintToConsole("No more fun. View console for details...");
        }
        else
        {
            Server.PrintToConsole("No more fun.");
        }

        foreach (var entity in breakables)
        {
            if (entity == null || !entity.IsValid) continue;

            // Gather info before breaking
            var origin = entity.AbsOrigin;
            string name = entity.Entity?.Name ?? "<unnamed>";
            int health = entity.Health;
            uint index = entity.Index;

            if (string.IsNullOrEmpty(name)) name = "<unnamed>";

            count++;

            string line = $"#{count} | Entity {index} | Name: {name} | HP: {health} | Pos: {origin?.X:F1}, {origin?.Y:F1}, {origin?.Z:F1}";

            if (player != null && player.IsValid)
            {
                player.PrintToConsole(line);
            }
            else
            {
                Server.PrintToConsole(line);
            }

            entity.AcceptInput("Break");
        }

        string summary = $"Broke {count} func_breakable entities.";

        if (player != null && player.IsValid)
        {
            player.PrintToConsole(summary);
        }
        else
        {
            Server.PrintToConsole(summary);
        }
    }
}
