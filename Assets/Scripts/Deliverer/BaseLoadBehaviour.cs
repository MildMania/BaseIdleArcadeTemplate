using System.Collections;
using System.Collections.Generic;
using MMFramework_2._0.PhaseSystem.Core.EventListener;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseLoadBehaviour : SerializedMonoBehaviour
{
    [SerializeField] protected EAttributeCategory _attributeCategory;

    [SerializeField] protected UpdatedFormationController _updatedFormationController;
    [SerializeField] protected Deliverer _deliverer;
    [SerializeField] protected bool _canLoadUnlimited;

    [HideIf("_canLoadUnlimited")] [SerializeField]
    protected Upgradable _loadCapacityUpgradable;

    [SerializeField] protected Upgradable _loadSpeedUpgradable;

    protected int _loadCapacity;

    protected float _loadDelay;
}

public abstract class BaseLoadBehaviour<TBaseProducer, TResource> : BaseLoadBehaviour
    where TBaseProducer : BaseProducer<TResource>
    where TResource : IResource
{
    private List<TBaseProducer> _producers = new List<TBaseProducer>();

    public Action OnCapacityFull;
    public Action OnCapacityEmpty;

    private void Awake()
    {
        if (!_canLoadUnlimited)
        {
            if (_loadCapacityUpgradable != null)
                _loadCapacityUpgradable.OnUpgraded += OnLoadCapacityUpgraded;
        }

        if(_loadSpeedUpgradable != null)
            _loadSpeedUpgradable.OnUpgraded += OnLoadSpeedUpgraded;
        
        OnAwakeCustomActions();
    }

    protected virtual void OnAwakeCustomActions()
    {
    }

    private void OnLoadCapacityUpgraded(UpgradableTrackData upgradableTrackData)
    {
        float value = GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, upgradableTrackData);

        _loadCapacity = (int) value;
    }

    private void OnDestroy()
    {
        if (!_canLoadUnlimited)
        {
            if (_loadCapacityUpgradable != null)
                _loadCapacityUpgradable.OnUpgraded -= OnLoadCapacityUpgraded;
        }
        if (_loadSpeedUpgradable != null)
            _loadSpeedUpgradable.OnUpgraded -= OnLoadSpeedUpgraded;
        
        OnDestroyCustomActions();
    }

    private void OnLoadSpeedUpgraded(UpgradableTrackData upgradableTrackData)
    {
        float value = GameConfigManager.Instance.GetAttributeUpgradeValue(_attributeCategory, upgradableTrackData);

        _loadDelay = 1 / value;
    }

    protected virtual void OnDestroyCustomActions()
    {
    }

    protected void OnProducerEnteredFieldOfView(TBaseProducer producer)
    {
        _producers.Add(producer);
    }

    protected void OnProducerExitedFieldOfView(TBaseProducer producer)
    {
        _producers.Remove(producer);
    }

    [PhaseListener(typeof(GamePhase), true)]
    public void OnGamePhaseStarted()
    {
        StartCoroutine(LoadRoutine());
    }

    private bool CanLoad()
    {
        if (_canLoadUnlimited)
        {
            return true;
        }

        bool result = _updatedFormationController.Container.childCount < _loadCapacity;

        return result;
    }

    private IEnumerator LoadRoutine()
    {
        float currentTime = 0;

        while (true)
        {
            currentTime += Time.deltaTime;

            if (currentTime > _loadDelay)
            {
                if (_updatedFormationController.Container.childCount == 0)
                    OnCapacityEmpty?.Invoke();
                else if (_updatedFormationController.Container.childCount == _loadCapacity)
                    OnCapacityFull?.Invoke();

                if (_producers.Count > 0 && CanLoad())
                {
                    int index = (int) Random.Range(0, _producers.Count - 0.1f);

                    TResource resource = default(TResource);
                    if (_producers[index].TryRemoveAndGetLastResource(ref resource))
                    {
                        LoadCustomActions(resource);
                    }
                }

                currentTime = 0;
            }

            yield return null;
        }
    }


    public abstract void LoadCustomActions(TResource resource);
}