using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceMovementBehaviour : MonoBehaviour
{
    [SerializeField] private BaseResource _resource;
    [SerializeField] private BaseResourceMovementCommand _resourceMovementCommand;
    
    public Action<BaseResource> OnMoveRoutineFinished { get; set; }

    private BaseResourceMovementCommand _clonedResourceMovementCommand;
    

    public void Move(Transform target, Transform container)
    {
        StopAllCoroutines();
        _clonedResourceMovementCommand = CloneResourceMovement(target, container);
        _clonedResourceMovementCommand.OnMovementCompleted += OnMovementCompleted;
        _clonedResourceMovementCommand.ExecuteMove();
    }
    
    private void OnMovementCompleted()
    {
        
        OnMoveRoutineFinished?.Invoke(_resource);
        _clonedResourceMovementCommand.OnMovementCompleted -= OnMovementCompleted;
    }

    private BaseResourceMovementCommand CloneResourceMovement(Transform target, Transform container)
    {
        var clonedResourceMovement = Instantiate(_resourceMovementCommand);
        clonedResourceMovement.Resource = _resource;
        clonedResourceMovement.Target = target;
        clonedResourceMovement.Container = container;
        
        return clonedResourceMovement;
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }
}