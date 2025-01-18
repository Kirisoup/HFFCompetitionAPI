global using UnityEngine;
global using Object = UnityEngine.Object;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.Networking.Match;

namespace HFFTournamentAPI;

// [BepInDependency(Timing.Plugin.GUID)]
[BepInPlugin(GUID, NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("Human.exe")]
internal sealed partial class Plugin : BaseUnityPlugin
{
    public const string NAME = nameof(HFFTournamentAPI);
    public const string GUID = $"hff.kirisoup.{NAME}";

    public static Plugin instance = null!;
    internal static new ManualLogSource Logger = null!;

	private static readonly GameObject? _obj = new($"{NAME}_{DateTime.UtcNow.Ticks}");

	Plugin() {
		instance ??= this;
		Logger ??= base.Logger;
		SubModules.ModuleAttribute.InitModules(_obj);
	}

	void Awake() {
		DontDestroyOnLoad(_obj);
	}

	void OnDestroy() {
		Destroy(_obj);
	}
}