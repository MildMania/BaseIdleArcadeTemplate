using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerProvider : Singleton<ConsumerProvider>
{
    Dictionary<System.Type, List<BaseConsumer>> _consumersByResource =
        new Dictionary<System.Type, List<BaseConsumer>>();
    Dictionary<System.Type, List<BaseConsumer>> _consumersByRecentlyUsedResource =
        new Dictionary<System.Type, List<BaseConsumer>>();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {

    }

    public void AddConsumer(BaseConsumer consumer, System.Type resourceType)
    {
        List<BaseConsumer> list;
        System.Type resType = resourceType;

        _consumersByResource.TryGetValue(resType, out list);

        if (list != null)
        {
            _consumersByResource[resType].Add(consumer);
        }
        else
        {
            _consumersByResource[resType] = new List<BaseConsumer>();
            _consumersByResource[resType].Add(consumer);

            _consumersByRecentlyUsedResource[resType] = new List<BaseConsumer>();
        }
    }

    public void RemoveConsumer(BaseConsumer consumer, System.Type resourceType)
    {
        _consumersByResource[resourceType].Remove(consumer);
    }

    public List<BaseConsumer> GetConsumers(System.Type resourceType)
    {
        List<BaseConsumer> list;
        _consumersByResource.TryGetValue(resourceType, out list);

        return list;
    }

    public List<BaseConsumer> GetAvailableConsumers(System.Type resourceType)
    {
        List<BaseConsumer> consumers;
        List<BaseConsumer> availableConsumers = new List<BaseConsumer>();

        _consumersByResource.TryGetValue(resourceType, out consumers);
        foreach (BaseConsumer consumer in consumers)
        {
            availableConsumers.Add(consumer);
        }

        if (availableConsumers != null)
        {
            for (int i = availableConsumers.Count - 1; i >= 0; i--)
            {
                if (availableConsumers[i].IsFull())
                {
                    availableConsumers.RemoveAt(i);
                }
            }
        }
        
        return availableConsumers;
    }

    public void ReserveConsumer(System.Type type, BaseConsumer consumer)
    {
        _consumersByRecentlyUsedResource[type].Add(consumer);
        _consumersByResource[type].Remove(consumer);
    }

    public void ReleaseConsumer(System.Type type, BaseConsumer consumer)
    {
        _consumersByResource[type].Add(consumer);
        _consumersByRecentlyUsedResource[type].Remove(consumer);
    }
}