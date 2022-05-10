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

    public Action<BaseResource> OnMoveRoutineFinished { get; set; }

    public abstract void Move(Transform target, Transform container);
}