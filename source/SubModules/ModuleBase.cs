using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace HFFTournamentAPI.SubModules;

public abstract class ModuleBase : MonoBehaviour
{
	protected ModuleBase() {
		if (GetType().GetCustomAttributes<ModuleAttribute>(inherit: false)?.ToArray()
			is not [{ NAME: var name }, ..])
		{
			Destroy(this);
			throw new MissingAttributeException(GetType());
		}
		Logger = new(BepInEx.Logging.Logger.CreateLogSource(name));
	}

	public sealed class MissingAttributeException(Type type) : InvalidOperationException(
		$"Can't create an instance of {type.FullName} because {nameof(ModuleAttribute)} attribute is missing.");

	protected internal LoggerAdapter Logger { get; }
}
