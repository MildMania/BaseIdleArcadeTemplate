using System;
using MMPoolingSystem;
using UnityEngine;


[RequireComponent(typeof(GOPoolObject))]
public abstract class BaseResource : MonoBehaviour
{
    [SerializeField] private BaseResourceMovementBehaviour _baseResourceMovementBehaviour;
    private GOPoolObject _goPoolObject;

    public GOPoolObject GoPoolObject
    {
        get
        {
            if (_goPoolObject == null)
                _goPoolObject = GetComponent<GOPoolObject>();

            return _goPoolObject;
        }
    }

    public Action<BaseResource> OnMovementFinished { get; set; }

    public virtual void Move(Transform target, Transform container,
        BaseResourceMovementBehaviour baseResourceMovementBehaviour = default)
    {
        if (baseResourceMovementBehaviour != null)
        {
            _baseResourceMovementBehaviour = baseResourceMovementBehaviour;
        }

        if (_baseResourceMovementBehaviour != null)
        {
            _baseResourceMovementBehaviour.Move(transform, target, container, (() => OnMovementFinished?.Invoke(this)));
        }
    }
}