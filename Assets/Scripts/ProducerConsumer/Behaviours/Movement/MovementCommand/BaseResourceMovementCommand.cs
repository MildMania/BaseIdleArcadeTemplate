using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseResourceMovementCommand : ScriptableObject
{
    public BaseResource Resource { get; set; }
    public Transform Target { get; set; }
    public Transform Container { get; set; }
    
    public Action OnMovementCompleted { get; set; }

    private Coroutine _moveRoutine;
    
    protected abstract IEnumerator MoveRoutine(Action onMovementCompleted);
    
    public void ExecuteMove()
    {
        _moveRoutine = CoroutineRunner.Instance.StartCoroutine(MoveRoutine(onMovementCompleted));

        void onMovementCompleted()
        {
            OnMovementCompleted?.Invoke();
            StopMove();
        }
    }

    private void StopMove()
    {
        CoroutineRunner.Instance.StopCoroutine(_moveRoutine);
        Destroy(this);
    }
}
