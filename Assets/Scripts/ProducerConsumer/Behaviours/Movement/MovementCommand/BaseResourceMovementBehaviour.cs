using System;
using UnityEngine;

public abstract class BaseResourceMovementBehaviour : ScriptableObject
{

    public virtual void Move(Transform sourceTransform, Transform targetTransform, Transform container, Action OnMovementFinished)
    {

        MoveCustomActions(sourceTransform, targetTransform, container, OnMovementFinished);
    }

    protected abstract void MoveCustomActions(Transform sourceTransform, Transform targetTransform, Transform container, Action OnMovementFinished);
}