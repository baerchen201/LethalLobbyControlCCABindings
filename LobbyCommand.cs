using System.Collections.Generic;
using ChatCommandAPI;
using ChatCommandAPI.Utils;
using UnityEngine;

namespace LobbyControlCCABindings;

public class LobbyCommand : Command
{
    public override string Name => nameof(LobbyControl);

    public override string Command => "lobby";

    public override string[] Syntax =>
        [
            "help",
            "status",
            "{ open | close }",
            "{ private | friend | public }",
            "rename <name>",
            "autosave",
            "{ save | load } [file]",
            "switch <file>",
            "{ clear | dropall }",
            "version",
        ];

    public override void Invoke(string _args)
    {
        var args = new List<string>(3) { Command };
        args.AddRange(Args.Parse(_args));
        if (args.Count <= 1)
            goto help;

        // some commands below expect this length and will fail if this isn't provided
        while (args.Count < 3)
            args.Add("");

        var node = ScriptableObject.CreateInstance<TerminalNode>();
        bool success;
        switch (args[1])
        {
            case "version":
                var message =
                    $"LobbyControl version {LobbyControlCCABindings.Instance.LobbyControlVersion}\n"
                    + $"bindings v{MyPluginInfo.PLUGIN_VERSION} for v{LobbyControl.MyPluginInfo.PLUGIN_VERSION}";
                if (
                    LobbyControlCCABindings.Instance.LobbyControlVersion
                    != LobbyControl.MyPluginInfo.PLUGIN_VERSION
                )
                    Chat.PrintWarning(message);
                else
                    Chat.Print(message);
                return;

            case "help":
                goto help;
            case "status":
                success = LobbyControl.TerminalCommands.LobbyCommand.StatusCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "open":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.OpenCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "close":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.CloseCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "private":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.PrivateCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "friend":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.FriendCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "public":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.PublicCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "rename":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.RenameCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "autosave":
                success = LobbyControl.TerminalCommands.LobbyCommand.AutoSaveCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "save":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.SaveCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "load":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.LoadCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "switch":
                // why? why not allow switching while landed?
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.SwitchCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "clear":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.ClearCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            case "dropall":
                verifyShipPhase();
                verifyCanModify();
                success = LobbyControl.TerminalCommands.LobbyCommand.DropAllCommand(
                    ref node,
                    args.ToArray()
                );
                break;
            default:
                throw new InvalidArgumentsException();
        }

        if (success)
            Chat.Print(node.displayText.Trim());
        else
            throw new CommandException(node.displayText.Trim());
        return;

        help:
        Chat.PrintWarning(
            "status : prints the current lobby status\n"
                + "open : open the lobby\n"
                + "close : close the lobby\n"
                + "private : set lobby to Invite Only\n"
                + "friend : set lobby to Friends Only\n"
                + "public : set lobby to Public\n"
                + "rename <name> : change the name of the lobby\n"
                + "autosave : toggle the autosave state\n"
                + "save [file] : save the lobby to file\n"
                + "load [file] : re-load the lobby from file\n"
                + "switch <file> : change the future save/load location without loading\n"
                + "clear : clear the current save\n"
                + "dropall : drop all items on the ground"
        );
        return;

        void verifyShipPhase()
        {
            if (!StartOfRound.Instance.inShipPhase)
                throw new CommandException("The ship must be in orbit");
        }

        void verifyCanModify()
        {
            if (!LobbyControl.LobbyControl.CanModifyLobby)
                throw new CommandException("You can not modify the lobby right now");
        }
    }
}
