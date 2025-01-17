namespace HFFCompetitionAPI.Timing;

public sealed partial class GameCycleCounter : MonoBehaviour
{
	private static GameObject? _parent;
	private static GameCycleCounter? _instance;
	public static GameCycleCounter Instance => _instance ?? Init();

	// ensures type can only be instantiated once
	GameCycleCounter() {
		var existing = FindObjectsOfType<GameCycleCounter>();
		if (existing is []) return;
		if (existing is [var obj] && obj == this) return;
		Destroy(this);
		throw new InvalidOperationException(
			$"Caught an attempt to instantiate a {nameof(GameCycleCounter)} multiple times");
	}

	public static GameCycleCounter Init() {
		if (_instance is not null) return _instance;
		_parent = new GameObject($"{nameof(GameCycleCounter)}_{DateTime.UtcNow.Ticks}");
		_instance = _parent.AddComponent<GameCycleCounter>();
		return _instance;
	}

	public ulong Cycles { get; private set; }
	void FixedUpdate() {
		unchecked { Cycles++; }
	}

	public static float CycleSpan { get; } = Time.fixedDeltaTime;

	void Awake() => DontDestroyOnLoad(_parent);
	void OnDestroy() => Destroy(_parent);

	public static bool Destroy() {
		if (_instance is null) return false;
		Destroy(_parent);
		Destroy(_instance);
		return true;
	}
}
