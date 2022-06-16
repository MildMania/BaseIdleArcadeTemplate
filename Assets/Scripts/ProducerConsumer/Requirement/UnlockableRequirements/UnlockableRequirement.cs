using System.Collections;
using System.Collections.Generic;
using MMFramework.Utilities;
using UnityEngine;
using MMFramework.TasksV2;

public class UnlockableRequirement : MonoBehaviour
{
    [SerializeField] private List<UnlockableObject> _unlockablesRequired;
    [SerializeField] protected List<GameObject> _unlockAfterRequirements;
    [SerializeField] protected GameObject _lockAfterRequirements;
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] protected MMTaskExecutor _poofTaskExecutor;
    [SerializeField] protected List<UnlockableRequirement> _nextUnlockableToInitList;
    [SerializeField] protected bool _initializeOnStart = false;

    protected int _necessaryUnlockCount;
    protected int _unlockedSoFar;

    protected void Start()
    {
        if (_initializeOnStart)
        {
            CoroutineRunner.Instance.StartCoroutine(InitRoutine());
        }
    }

    protected IEnumerator InitRoutine()
    {
        if (_unlockAfterRequirements != null)
        {
            foreach (GameObject gameObjToUnlock in _unlockAfterRequirements)
            {
                gameObjToUnlock.SetActive(false);
            }
        }

        if (_lockAfterRequirements != null)
        {
            _lockAfterRequirements.SetActive(true);
        }

        _unlockedSoFar = 0;
        SetRequirementCount();

        foreach (UnlockableObject unlockableGo in _unlockablesRequired)
        {
            while (unlockableGo.TrackData == null)
            {
                yield return null;
            }
            if (unlockableGo.TrackData.IsUnlock)
            {
                _unlockedSoFar++;
            }
            else
            {
                unlockableGo.OnUnlocked += OnRequirementMet;
            }
        }

        if (_unlockedSoFar >= _necessaryUnlockCount)
        {
            SetGameObjectsActive();
        }
    }

    private void OnRequirementMet()
    {
        _unlockedSoFar++;
        if (_unlockedSoFar == _necessaryUnlockCount)
        {
            if (_lookAtTarget != null)
            {
                VirtualCameraUnlock vCam = (VirtualCameraUnlock)CameraManager.Instance.GetCamera(ECameraType.Unlock);
                vCam.SetFollowTarget(_lookAtTarget);
                CameraManager.Instance.ActivateCamera(new CameraActivationArgs(ECameraType.Unlock));

                WorldInputManager.Instance.gameObject.SetActive(false);

                CoroutineRunner.Instance.WaitForSeconds(1f, SetGameObjectsActive);

                CoroutineRunner.Instance.WaitForSeconds(2f, () =>
                {
                    CameraManager.Instance.ActivateCamera(new CameraActivationArgs(ECameraType.Game));
                    WorldInputManager.Instance.gameObject.SetActive(true);
                });
            }
            else
            {
                SetGameObjectsActive();
            }
        }
    }

    protected virtual void SetRequirementCount()
    {
        _necessaryUnlockCount = _unlockablesRequired.Count;
    }

    private void SetGameObjectsActive()
    {
        if (_lockAfterRequirements != null)
        {
            _poofTaskExecutor?.Execute(this);
            _lockAfterRequirements.SetActive(false);
        }

        if (_unlockAfterRequirements != null)
        {
            foreach (GameObject gameObjToUnlock in _unlockAfterRequirements)
            {
                gameObjToUnlock.SetActive(true);
            }
        }

        InitNext();
    }

    private void OnDestroy()
    {
        foreach (UnlockableObject unlockableGo in _unlockablesRequired)
        {
            unlockableGo.OnUnlocked -= OnRequirementMet;
        }
    }

    protected void InitNext()
    {
        foreach (var nextUnlockableRequirement in _nextUnlockableToInitList)
        {
            CoroutineRunner.Instance.StartCoroutine(nextUnlockableRequirement.InitRoutine());
        }
    }
}

