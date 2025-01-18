global using UnityEngine;
global using Object = UnityEngine.Object;
using BepInEx;
using BepInEx.Logging;

namespace HFFTournamentAPI;

[BepInDependency(Timing.Plugin.GUID)]
[BepInPlugin(GUID, NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("Human.exe")]
internal sealed partial class Plugin : BaseUnityPlugin
{
    public const string NAME = nameof(HFFTournamentAPI);
    public const string GUID = $"hff.kirisoup.{NAME}";

    public static Plugin instance = null!;
    internal static new ManualLogSource Logger = null!;

	Plugin() {
		instance ??= this;
		Logger ??= base.Logger;
	}

	void OnDestroy() {
	}
}