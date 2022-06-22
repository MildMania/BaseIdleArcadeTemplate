using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpResourceMovementBehaviour",
    menuName = "ScriptableObjects/IdleArcade/Resource/Movement/JumpResourceMovementBehaviour",
    order = 1)]
public class JumpResourceMovementBehaviour : BaseResourceMovementBehaviour
{
    [SerializeField] private float _moveDuration;
    [SerializeField] private int _jumpPower;

    private float _distanceThreshold = 1f;

    private Coroutine _coroutine;

    protected override void MoveCustomActions(Transform sourceTransform, Transform targetTransform, Transform container,
        Action OnMovementFinished)
    {
        if (_coroutine != null)
        {
            CoroutineRunner.Instance.StopCoroutine(_coroutine);
        }

        _coroutine = CoroutineRunner.Instance.StartCoroutine(MoveRoutine(sourceTransform, targetTransform, container,
            OnMovementFinished));
    }


    protected IEnumerator MoveRoutine(Transform sourceTransform, Transform targetTransform, Transform container,
        Action OnMovementFinished)
    {
        float currentTime = 0f;


        var producibleTransform = sourceTransform;
        producibleTransform.SetParent(container);


        producibleTransform.DOJump(targetTransform.position, _jumpPower, 1, _moveDuration);

        while (currentTime < _moveDuration)
        {
            if (targetTransform == null)
            {
                yield return null;
                break;
            }

            float step = currentTime / _moveDuration;

            producibleTransform.rotation =
                Quaternion.Lerp(producibleTransform.rotation, targetTransform.rotation, step);

            currentTime += Time.deltaTime;
            yield return null;
        }

        if (targetTransform != null && Vector3.Distance(producibleTransform.position, targetTransform.position) >
            _distanceThreshold)
        {
            currentTime = 0f;

            while (currentTime < _moveDuration)
            {
                if (targetTransform == null)
                {
                    yield return null;
                    break;
                }

                float step = currentTime / _moveDuration;

                producibleTransform.position
                    = Vector3.Lerp(
                        producibleTransform.position,
                        targetTransform.position,
                        step);

                producibleTransform.rotation =
                    Quaternion.Lerp(producibleTransform.rotation, targetTransform.rotation, step);


                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        if (targetTransform != null)
        {
            producibleTransform.position = targetTransform.position;
            producibleTransform.rotation = targetTransform.rotation;
        }


        OnMovementFinished?.Invoke();
    }
}