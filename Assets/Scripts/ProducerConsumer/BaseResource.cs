using System;
using MMPoolingSystem;
using UnityEngine;


[RequireComponent(typeof(GOPoolObject))]
public abstract class BaseResource : MonoBehaviour
{
    private GOPoolObject _goPoolObject;


    public GOPoolObject GOPoolObject
    {
        get
        {
            if (_goPoolObject == null)
                _goPoolObject = GetComponent<GOPoolObject>();

            return _goPoolObject;
        }
    }

    public ResourceProduceMovementBehaviour ProduceMovementBehaviour =>
        GetComponentInChildren<ResourceProduceMovementBehaviour>();

    public ResourceConsumeMovementBehaviour ConsumeMovementBehaviour =>
        GetComponentInChildren<ResourceConsumeMovementBehaviour>();

    public ResourceLoadMovementBehaviour LoadMovementBehaviour =>
        GetComponentInChildren<ResourceLoadMovementBehaviour>();

    public ResourceUnloadMovementBehaviour UnloadMovementBehaviour =>
        GetComponentInChildren<ResourceUnloadMovementBehaviour>();
}