using System;
using System.Collections;
using System.Collections.Generic;
using MMFramework.TasksV2;
using MMFramework.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using Unity.Mathematics;

public class UnlockableObject : SerializedMonoBehaviour, IUnlockable
{
	[OdinSerialize] public Unlockable Unlockable { get; private set; } = new Unlockable();

	[SerializeField] private Guid _guid;

	[SerializeField] private List<GameObject> _unlockableGOList;

	[SerializeField] private GameObject _lockObjects;

	[SerializeField] private BaseCharacterDetector _baseCharacterDetector;

	[SerializeField] private int _decreaseAmount = 10;
	
    [SerializeField] private Collider _unwalkableCollider;

    [SerializeField] private Rigidbody _characterRb;

	[SerializeField]
	protected iOSHapticFeedback.iOSFeedbackType _hapticType = iOSHapticFeedback.iOSFeedbackType.ImpactLight;

	protected OnHapticRequestedEventRaiser _onHapticRequestedEventRaiser = new OnHapticRequestedEventRaiser();

	protected UnlockableTrackData _unlockableTrackData;
    public UnlockableTrackData TrackData => _unlockableTrackData;

	public Action<UnlockableTrackData> OnUnlockableInit;

	public Action<int, UnlockableTrackData> OnTryUnlock;
    public Action OnUnlocked;

	public MMTaskExecutor OnUnlockTasks;
	
	private Coroutine _unlockRoutine;

    private int _bitmask;

    [SerializeField] private bool _updateAstarImmediate = false;

	[SerializeField] private UnlockableAnimationController _unlockableAnimationController;

	private void Awake()
	{
		_baseCharacterDetector.OnDetected += OnDetected;
		_baseCharacterDetector.OnEnded += OnEnded;
		Unlockable.OnLockedValueChanged += OnLockedValueChanged;
		// 6 is character layer and 17 is ai agent layer
        _bitmask = (1 << 6) | (1 << 17);
    }

	private void OnEnable()
	{
		if (_unlockableTrackData != null)
		{
			gameObject.SetActive(!_unlockableTrackData.IsUnlock);
		}
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

		foreach (var unlockableGO in _unlockableGOList)
		{
			unlockableGO.SetActive(_unlockableTrackData.IsUnlock);
		}
		
        if (_unlockableTrackData.IsUnlock && _unwalkableCollider != null)
        {
            UpdateAstarGraph();
        }

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
		_unlockableAnimationController.PlayAnimation(EUnlockableAnimation.Update);
	}

	private void OnEnded(Character character)
	{
		StopCoroutine(_unlockRoutine);
		_unlockableAnimationController.PlayAnimation(EUnlockableAnimation.Idle);
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
		
		var userCoinInventoryData = UserManager.Instance.LocalUser.GetUserData<UserCoinInventoryData>();
		Coin trackableCoin;
		userCoinInventoryData.Tracker.TryGetSingle(ECoin.Gold, out trackableCoin);

		int amount=_decreaseAmount;
		if (trackableCoin.TrackData.CurrentCount < _decreaseAmount)
		{
			amount = trackableCoin.TrackData.CurrentCount;
		}

		if (trackableCoin.TrackData.CurrentCount == 0)
		{
			_unlockableAnimationController.PlayAnimation(EUnlockableAnimation.Idle);
			return false;
		}
		
		if (Unlockable.TryUnlock(UserManager.Instance.LocalUser, amount))
		{
			foreach (var unlockableGO in _unlockableGOList)
			{
				unlockableGO.SetActive(true);
			}
			
			if (_lockObjects != null)
			{
				_lockObjects.SetActive(false);
			}

            if (_unwalkableCollider != null)
            {
                CoroutineRunner.Instance.WaitForSeconds(GetAstarDelay(), UpdateAstarGraph);
            }

			OnDetectedCustomActions();
			
			OnUnlocked?.Invoke();
			OnUnlockTasks?.Execute(this);
			isUnlock = true;
			_unlockableAnimationController.PlayAnimation(EUnlockableAnimation.Disappear);
			CoroutineRunner.Instance.WaitForSeconds(0.5f, () =>
			{
				gameObject.SetActive(false);
			});
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
			Logger.Log("SAVED!!!");
		}

		return isUnlock;
	}

    private float GetAstarDelay()
    {
        if (_updateAstarImmediate)
        {
            return 0.01f;
        }

        return 0.5f;
    }

    private void OnDetected(Character character)
	{
		_unlockRoutine = StartCoroutine(UnlockRoutine(character));
        _characterRb = character.CharacterRb;
    }

    private void MoveCharacterToNearestWalkable()
    {
        if (_characterRb != null && Physics.CheckBox(_unwalkableCollider.bounds.center, _unwalkableCollider.bounds.extents, Quaternion.identity, _bitmask))
        {
            GraphNode node;
			//Logger.Log($"character position BEFORE update {_characterRb.transform.position}");
            CoroutineRunner.Instance.WaitForSeconds(0.1f, () =>
            {
                node = AstarPath.active.GetNearest(_characterRb.transform.position, NNConstraint.Default).node;
                Vector3 transportPos = (Vector3)node.position;
				//Logger.Log($"character position AFTER update {transportPos}");
				_characterRb.transform.DOMove(transportPos, 0.4f);
            });
        }
        else
        {
			Logger.LogWarning("Character rigid body cannot be found");
        }
    }

    private void UpdateAstarGraph()
    {
        GraphUpdateObject guo = new GraphUpdateObject(_unwalkableCollider.bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
        MoveCharacterToNearestWalkable();
    }

	protected virtual void OnDetectedCustomActions()
	{
	}
}