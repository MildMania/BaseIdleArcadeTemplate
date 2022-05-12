using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Random = UnityEngine.Random;

public class AIHelper : SerializedMonoBehaviour
{
    [OdinSerialize] private List<BaseResource> _resources;

    private BaseResource _resource;
    public BaseResource Resource => _resource;

    [OdinSerialize] private Dictionary<BaseResource, BaseLoadBehaviour> _resourceToLoadBehaviour;
    [OdinSerialize] private Dictionary<BaseResource, BaseUnloadBehaviour> _resourceToUnloadBehaviour;

    private BaseLoadBehaviour _currentLoadBehaviour;
    public BaseLoadBehaviour CurrentLoadBehaviour => _currentLoadBehaviour;

    private BaseUnloadBehaviour _currentUnloadBehaviour;
    public BaseUnloadBehaviour CurrentUnloadBehaviour => _currentUnloadBehaviour;


    private void Start()
    {
        InitAiHelper();
    }


    private void InitAiHelper()
    {
        foreach (var item in _resourceToLoadBehaviour)
        {
            if (item.Key.Equals(_resource))
            {
                _currentLoadBehaviour = item.Value;
            }

            item.Value.Deactivate();
        }

        foreach (var item in _resourceToUnloadBehaviour)
        {
            if (item.Key.Equals(_resource))
            {
                _currentUnloadBehaviour = item.Value;
            }

            item.Value.Deactivate();
        }
    }

    public void PickRandomLoadUnloadBehaviour(Action onRandomLoadUnloadSelected)
    {
        StartCoroutine(PickRandomLoadUnloadRoutine(onRandomLoadUnloadSelected));
    }

    private IEnumerator PickRandomLoadUnloadRoutine(Action onRandomLoadUnloadSelected)
    {
        do
        {
            int randomResourceIndex = Random.Range(0, _resources.Count);
            _resource = _resources[randomResourceIndex];

            yield return null;
        } while (ProducerProvider.Instance.GetProducers(_resource.GetType()) == null ||
                 ProducerProvider.Instance.GetProducers(_resource.GetType()).Count < 1 ||
                 ConsumerProvider.Instance.GetConsumers(_resource.GetType()) == null ||
                 ConsumerProvider.Instance.GetConsumers(_resource.GetType()).Count < 1);

        InitAiHelper();
        onRandomLoadUnloadSelected?.Invoke();
    }

    public void UpdateLoadUnloadBehaviours()
    {
        foreach (var item in _resourceToLoadBehaviour)
        {
            if (item.Key.Equals(_resource))
            {
                _currentLoadBehaviour = item.Value;
            }
        }

        foreach (var item in _resourceToUnloadBehaviour)
        {
            if (item.Key.Equals(_resource))
            {
                _currentUnloadBehaviour = item.Value;
            }
        }
    }
}