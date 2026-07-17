using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;

namespace LobbyControlCCABindings;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LobbyControl.MyPluginInfo.PLUGIN_GUID)]
[BepInDependency(ChatCommandAPI.MyPluginInfo.PLUGIN_GUID)]
public class LobbyControlCCABindings : BaseUnityPlugin
{
    public static LobbyControlCCABindings Instance { get; private set; } = null!;
    internal static new ManualLogSource Logger { get; private set; } = null!;

    internal string LobbyControlVersion = "[UNKNOWN]";

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        if (
            !Chainloader.PluginInfos.TryGetValue(
                LobbyControl.MyPluginInfo.PLUGIN_GUID,
                out var pluginInfo
            )
            || (LobbyControlVersion = pluginInfo.Metadata.Version.ToString(3))
                != LobbyControl.MyPluginInfo.PLUGIN_VERSION
        )
            Logger.LogWarning(
                $"LobbyControl version differs: expected {LobbyControl.MyPluginInfo.PLUGIN_VERSION}, got {LobbyControlVersion}"
            );

        _ = new LobbyCommand();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
}
