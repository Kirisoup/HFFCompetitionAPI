using BepInEx.Logging;

namespace HFFTournamentAPI.SubModules;

public sealed class LoggerAdapter
{
	private readonly ManualLogSource _logger;
	internal LoggerAdapter(ManualLogSource logger) => _logger = logger;

	public void LogFatal(object data) => _logger.LogFatal(data);
	public void LogError(object data) => _logger.LogError(data);
	public void LogWarning(object data) => _logger.LogWarning(data);
	public void LogMessage(object data) => _logger.LogMessage(data);
	public void LogInfo(object data) => _logger.LogInfo(data);
	public void LogDebug(object data) => _logger.LogDebug(data);
}