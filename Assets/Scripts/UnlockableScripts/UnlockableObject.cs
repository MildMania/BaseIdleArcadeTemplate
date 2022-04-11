using System;
using System.Collections;
using MMFramework.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class UnlockableObject : SerializedMonoBehaviour, IUnlockable
{
	[OdinSerialize] public Unlockable Unlockable { get; private set; } = new Unlockable();

	[SerializeField] private Guid _guid;

	[SerializeField] private GameObject _unlockableGO;

	[SerializeField] private GameObject _lockObjects;

	[SerializeField] private BaseCharacterDetector _baseCharacterDetector;

	[SerializeField]
	protected iOSHapticFeedback.iOSFeedbackType _hapticType = iOSHapticFeedback.iOSFeedbackType.ImpactLight;

	protected OnHapticRequestedEventRaiser _onHapticRequestedEventRaiser = new OnHapticRequestedEventRaiser();

	protected UnlockableTrackData _unlockableTrackData;

	public Action<UnlockableTrackData> OnUnlockableInit;

	public Action<int, UnlockableTrackData> OnTryUnlock;

	private Coroutine _unlockRoutine;


	private void Awake()
	{
		_baseCharacterDetector.OnDetected += OnDetected;
		_baseCharacterDetector.OnEnded += OnEnded;
		Unlockable.OnLockedValueChanged += OnLockedValueChanged;
	}

	private void Start()
	{
		UnlockableTrackable unlockableTrackable;

		if (UserManager.Instance.LocalUser.GetUserData<UserUnlockableData>().Tracker
			.TryGetSingle(_guid, out unlockableTrackable))
		{
			_unlockableTrackData = unlockableTrackable.TrackData;
		}
		else
		{
			_unlockableTrackData = new UnlockableTrackData(_guid, 0, false);
			unlockableTrackable = new UnlockableTrackable(_unlockableTrackData);
			UserManager.Instance.LocalUser.GetUserData<UserUnlockableData>().Tracker.TryCreate(unlockableTrackable);
		}

		Unlockable.Init(_unlockableTrackData);

		OnUnlockableInit?.Invoke(_unlockableTrackData);

		_unlockableGO.SetActive(_unlockableTrackData.IsUnlock);
		if (_lockObjects != null)
		{
			_lockObjects.SetActive(!_unlockableTrackData.IsUnlock);
		}

		gameObject.SetActive(!_unlockableTrackData.IsUnlock);

		OnStartCustomActions();
	}

	protected virtual void OnStartCustomActions()
	{
	}

	private void OnDestroy()
	{
		_baseCharacterDetector.OnDetected -= OnDetected;
		_baseCharacterDetector.OnEnded -= OnEnded;
		Unlockable.OnLockedValueChanged -= OnLockedValueChanged;
	}

	private void OnLockedValueChanged()
	{
		_onHapticRequestedEventRaiser.Raise(new OnHapticRequestedEventArgs(_hapticType));
	}

	private void OnEnded(Character character)
	{
		StopCoroutine(_unlockRoutine);
	}

	private IEnumerator UnlockRoutine(Character character)
	{
		while (true)
		{
			var movementIdleState = character.GetComponentInChildren<MovementIdleState>();

			if (movementIdleState == null)
			{
				yield return null;
				continue;
			}

			if (!movementIdleState.IsOnIdleState())
			{
				yield return null;
				continue;
			}

			if (TryToUnlock(character))
			{
				yield break;
			}

			yield return null;
		}
	}

	private bool TryToUnlock(Character character)
	{
		bool isUnlock;
		int oldValue = Unlockable.GetRequirementCoin() - _unlockableTrackData.CurrentCount;
		if (Unlockable.TryUnlock(UserManager.Instance.LocalUser, 1))
		{
			_unlockableGO.SetActive(true);
			gameObject.SetActive(false);
			if (_lockObjects != null)
			{
				_lockObjects.SetActive(false);
			}

			OnDetectedCustomActions();
			
			isUnlock = true;
		}
		else
		{
			isUnlock = false;
		}

		OnTryUnlock?.Invoke(oldValue, _unlockableTrackData);

		var coinController = character.GetComponentInChildren<CoinController>();
		coinController.UpdateCoinCount();

		UserManager.Instance.LocalUser.SaveData(onSavedCallback);

		void onSavedCallback()
		{
			Debug.Log("SAVED!!!");
		}

		return isUnlock;
	}

	private void OnDetected(Character character)
	{
		_unlockRoutine = StartCoroutine(UnlockRoutine(character));
	}

	protected virtual void OnDetectedCustomActions()
	{
	}
}