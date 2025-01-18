global using UnityEngine;
using HFFTournamentAPI.SubModules;

namespace HFFTournamentAPI.Timing;

[Module(nameof(Timing))]
public sealed class Module : ModuleBase
{
    public static Module instance = null!;
    internal static new LoggerAdapter Logger = null!;

	Module() {
		instance ??= this;
		Logger ??= base.Logger;
		Logger.LogInfo(nameof(Timing));
	}

	void Awake() {
		GameCycleCounter.Init();
	}

	void OnDestroy() {
		Logger.LogInfo($"Destroying {nameof(Timing)}");
		Logger.LogInfo(GameCycleCounter.Destroy());
	}
}