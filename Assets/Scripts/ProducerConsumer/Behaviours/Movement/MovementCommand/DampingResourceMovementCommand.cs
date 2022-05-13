using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(fileName = "DampingResourceMovementCommand",
    menuName = "ScriptableObjects/IdleArcade/Resource/Movement/Damping",
    order = 2)]
public class DampingResourceMovementCommand : BaseResourceMovementCommand
{
    [SerializeField] private float _moveDuration;
    [SerializeField] private int _jumpPower;

    private float _distanceThreshold = 1f;

    protected override IEnumerator MoveRoutine(Action onMovementCompleted)
    {
        float currentTime = 0f;


        var producibleTransform = Resource.transform;
        producibleTransform.SetParent(Container);


        producibleTransform.DOJump(Target.position, _jumpPower, 1, _moveDuration);

        while (currentTime < _moveDuration)
        {
            if (Target == null)
            {
                yield return null;
                break;
            }

            float step = currentTime / _moveDuration;

            producibleTransform.rotation = Quaternion.Lerp(producibleTransform.rotation, Target.rotation, step);

            currentTime += Time.deltaTime;
            yield return null;
        }

        if (Target != null && Vector3.Distance(producibleTransform.position, Target.position) > _distanceThreshold)
        {
            currentTime = 0f;

            while (currentTime < _moveDuration)
            {
                if (Target == null)
                {
                    yield return null;
                    break;
                }

                float step = currentTime / _moveDuration;

                producibleTransform.position
                    = Vector3.Lerp(
                        producibleTransform.position,
                        Target.position,
                        step);

                producibleTransform.rotation = Quaternion.Lerp(producibleTransform.rotation, Target.rotation, step);


                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        if (Target != null)
        {
            producibleTransform.position = Target.position;
            producibleTransform.rotation = Target.rotation;
        }


        onMovementCompleted?.Invoke();
    }
}