using System;
using System.Collections;
using System.Collections.Generic;
using MMFramework_2._0.PhaseSystem.Core.EventListener;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseLoadBehaviour : SerializedMonoBehaviour
{
    [SerializeField] protected EAttributeCategory _attributeCategory;
    
    [SerializeField] protected EUpgradable _loadSpeedUpgradeType;
    [SerializeField] protected EUpgradable _loadCapacityUpgradeType;
    
    [SerializeField] protected UpdatedFormationController _updatedFormationController;
    [SerializeField] protected Deliverer _deliverer;
    [SerializeField] protected bool _canLoadUnlimited;

    [SerializeField] protected Transform _container;

    [SerializeField] protected bool _isActiveOnStart = true;

    [SerializeField] protected iOSHapticFeedback.iOSFeedbackType _hapticType = iOSHapticFeedback.iOSFeedbackType.ImpactMedium;

    protected OnHapticRequestedEventRaiser _onHapticRequestedEventRaiser = new OnHapticRequestedEventRaiser();


    protected Upgradable _loadCapacityUpgradable;

    protected Upgradable _loadSpeedUpgradable;

    protected int _loadCapacity;

    protected float _loadDelay;

    public Action OnCapacityEmpty;
    public Action OnCapacityFull;

    protected bool _isActive = true;
    protected bool _isUpgradableActive = true;
    public virtual void StopLoading()
    {
    }

    protected abstract IEnumerator LoadRoutine();

    public void Activate()
    {
        _isActive = true;
        StartCoroutine(LoadRoutine());
    }

    public void Deactivate()
    {
        _isActive = false;
        StopAllCoroutines();
    }

    //TODO: Consider controlling loading/unloading using start/stop loading methods!
}

public abstract class BaseLoadBehaviour<TBaseProducer, TResource> : BaseLoadBehaviour
    where TBaseProducer : BaseProducer<TResource>
    where TResource : IResource
{
    private List<TBaseProducer> _producers = new List<TBaseProducer>();


    private void Awake()
    {
        OnAwakeCustomActions();
    }

    private void Start()
    {
        if (_isUpgradableActive)
        {
            _loadSpeedUpgradable = UpgradableManager.Instance.GetUpgradable(_attributeCategory, _loadSpeedUpgradeType);
            _loadDelay = 1 / GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, _loadSpeedUpgradable.UpgradableTrackData);
            _loadSpeedUpgradable.OnUpgraded += OnLoadSpeedUpgraded;
        }

        if (!_canLoadUnlimited)
        {
            _loadCapacityUpgradable =
                UpgradableManager.Instance.GetUpgradable(_attributeCategory, _loadCapacityUpgradeType);
            
            _loadCapacity = (int) GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory,
                _loadCapacityUpgradable.UpgradableTrackData);
            
            _loadCapacityUpgradable.OnUpgraded += OnLoadCapacityUpgraded;
        }

        if(_isActiveOnStart)
            Activate();
    }


    protected virtual void OnAwakeCustomActions()
    {
    }

    private void OnLoadCapacityUpgraded(UpgradableTrackData upgradableTrackData)
    {
        float value =
            GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, upgradableTrackData);

        _loadCapacity = (int) value;
    }

    private void OnDestroy()
    {
        StopLoading();
    }

    private void OnLoadSpeedUpgraded(UpgradableTrackData upgradableTrackData)
    {
        float value =
            GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, upgradableTrackData);

        _loadDelay = 1 / value;
    }

    protected virtual void OnDestroyCustomActions()
    {
        Deactivate();
    }

    protected void OnProducerEnteredFieldOfView(TBaseProducer producer)
    {
        _producers.Add(producer);
    }

    protected void OnProducerExitedFieldOfView(TBaseProducer producer)
    {
        _producers.Remove(producer);
    }


    private bool CanLoad()
    {
        if (_canLoadUnlimited)
        {
            return true;
        }

        return _deliverer.Container.childCount < _loadCapacity;
    }

    protected override IEnumerator LoadRoutine()
    {
        float currentTime = 0;

        while (true)
        {
            currentTime += Time.deltaTime;

            if (_isActive && currentTime > _loadDelay)
            {
                if (_container.childCount == 0)
                    OnCapacityEmpty?.Invoke();
                else if (_container.childCount == _loadCapacity)
                    OnCapacityFull?.Invoke();

                if (_producers.Count > 0 && CanLoad())
                {
                    int index = (int) Random.Range(0, _producers.Count - 0.1f);

                    TResource resource = default(TResource);
                    if (_producers[index].TryRemoveAndGetLastResource(ref resource))
                    {
                        if (_isActiveOnStart)
                        {
                            _onHapticRequestedEventRaiser.Raise(new OnHapticRequestedEventArgs(_hapticType));
                        }
                        LoadCustomActions(resource);
                        _deliverer.OnContainerEmpty?.Invoke(_deliverer.Container.childCount == 0);
                    }
                }

                currentTime = 0;
            }

            yield return null;
        }
    }


    public override void StopLoading()
    {
        base.StopLoading();
        
        if (!_canLoadUnlimited)
        {
            _loadCapacityUpgradable.OnUpgraded -= OnLoadCapacityUpgraded;
            
        }

        if (_loadSpeedUpgradable != null)
        {
            _loadSpeedUpgradable.OnUpgraded -= OnLoadSpeedUpgraded;
        }


        OnDestroyCustomActions();
    }

    public abstract void LoadCustomActions(TResource resource);
}