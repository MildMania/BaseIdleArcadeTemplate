using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(fileName = "LoosyResourceMovementCommand",
    menuName = "ScriptableObjects/IdleArcade/Resource/Movement/Loosy",
    order = 1)]
public class LoosyResourceMovementCommand : BaseResourceMovementCommand
{
    [SerializeField] private float _moveDuration;

    protected override IEnumerator MoveRoutine(Action onMovementCompleted)
    {
        float currentTime = 0;

        var producibleTransform = Resource.transform;
        Vector3 position = producibleTransform.position;
        Quaternion rotation = producibleTransform.rotation;
        producibleTransform.SetParent(Container);

        while (currentTime < _moveDuration)
        {
            float step = currentTime / _moveDuration;

            if (Target == null)
            {
                yield break;
            }
            
            producibleTransform.position
                = Vector3.Lerp(
                    position,
                    Target.position,
                    step);
            
            producibleTransform.rotation = Quaternion.Lerp(rotation, Target.rotation, step);


            currentTime += Time.deltaTime;
            yield return null;
        }

        producibleTransform.position = Target.position;
        producibleTransform.rotation = Target.rotation;

        
        onMovementCompleted?.Invoke();
    }
}