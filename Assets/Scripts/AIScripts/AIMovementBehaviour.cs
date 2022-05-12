using System;
using System.Collections;
using UnityEngine;
using Pathfinding;

public class AIMovementBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _rootTransform;

    [SerializeField] private Seeker _seeker;
    [SerializeField] private AIHelperAIPath _aIPath;
    
    [SerializeField] private EAttributeCategory _attributeCategory;
    [SerializeField] private EUpgradable _speedUpgradableType;

    private float _movementSpeed;

    private Action _currentPathCompletedCallback;
    private Action _currentPathStuckedCallback;

    private Upgradable _helperSpeedUpgradable;

    private bool _isMovementStarted;

    private float _minVelocity = 0.5f;
    private float _maxStopDuration = 5f;
    private float _maxPathDuration = 10f;

    public float _currentStopDuration = 0;

    public float _totalPathDuration = 0;
    
    private void Awake()
    {
        _aIPath.OnPathCompleted += OnPathCompleted;
        
    }

    private void Start()
    {
        _helperSpeedUpgradable = UpgradableManager.Instance.GetUpgradable(_attributeCategory, _speedUpgradableType);

        _movementSpeed = GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, _helperSpeedUpgradable.UpgradableTrackData);
        _aIPath.maxSpeed = _movementSpeed;
        
        _helperSpeedUpgradable.OnUpgraded += OnUpgradableUpgraded;
    }

    private void OnDestroy()
    {
        _helperSpeedUpgradable.OnUpgraded -= OnUpgradableUpgraded;
    }

    private void OnUpgradableUpgraded(UpgradableTrackData upgradableTrackData)
    {
        float value = GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, upgradableTrackData);

        _movementSpeed = value;
        _aIPath.maxSpeed = _movementSpeed;
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        var path = _seeker.StartPath(_rootTransform.position, targetPos);
        yield return StartCoroutine(path.WaitForPath());
    }

    public void MoveDestination(Vector3 targetPos, Action onPathCompletedCallback = null,
        Action onPathStuckedCallback = null)
    {
        _aIPath.isStopped = false;

        _isMovementStarted = true;

        _currentPathCompletedCallback = onPathCompletedCallback;
        _currentPathStuckedCallback = onPathStuckedCallback;

        StartCoroutine(MoveRoutine(targetPos));
    }

    public void Stop()
    {
        StopAllCoroutines();
        _aIPath.isStopped = true;
    }

    private void OnPathCompleted()
    {
        if(_currentPathCompletedCallback != null)
            _currentPathCompletedCallback();

        _isMovementStarted = false;
    }

    private void Update()
    {
        if (_isMovementStarted)
        {
            if(_totalPathDuration > _maxPathDuration || _currentStopDuration > _maxStopDuration)
            {
                _isMovementStarted = false;

                _currentStopDuration = 0;
                _totalPathDuration = 0;

                if(_currentPathStuckedCallback != null)
                    _currentPathStuckedCallback();
            }

            if (_aIPath.velocity.magnitude < _minVelocity)
            {
                _currentStopDuration += Time.deltaTime;

                Debug.Log(_totalPathDuration);
            }
            else
            {
                _currentStopDuration = 0;
            }

            _totalPathDuration += Time.deltaTime;
        }
        else
        {
            _currentStopDuration = 0;
            _totalPathDuration = 0;
        }
    }


}
