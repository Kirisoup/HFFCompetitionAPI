global using UnityEngine;
global using Object = UnityEngine.Object;
using BepInEx;
using BepInEx.Logging;

namespace HFFTournamentAPI.Kirisoup.PluginTemplate;

[BepInPlugin(GUID, NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("Human.exe")]
internal sealed partial class Plugin : BaseUnityPlugin
{
    public const string NAME = nameof(Kirisoup.PluginTemplate);
    public const string GUID = $"hff.kirisoup.{NAME}";

    public static Plugin instance = null!;
    internal static new ManualLogSource Logger = null!;

	Plugin() {
		instance ??= this;
		Logger ??= base.Logger;
	}
}