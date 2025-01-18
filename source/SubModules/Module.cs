using BepInEx.Logging;
using UnityEngine;

namespace HFFTournamentAPI.SubModules;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ModuleAttribute(string NAME) : Attribute
{
	public string NAME { get; } = NAME;

	internal static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("awwdaawdw");

	public static void InitModules(GameObject? obj) => AppDomain.CurrentDomain.GetAssemblies()
		.SelectMany(assembly => assembly.GetTypes().Select(type => (assembly, type)))
		.Where(item => item.type.BaseType == typeof(ModuleBase))
		.ToList()
		.ForEach(item => {
			try {
				var component = obj?.AddComponent(item.type);
			} catch (ModuleBase.MissingAttributeException e) {
				Logger.LogError(e.Message);
			}
		});
}
