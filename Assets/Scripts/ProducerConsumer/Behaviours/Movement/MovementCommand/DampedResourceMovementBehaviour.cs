using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DampedResourceMovementBehaviour",
    menuName = "ScriptableObjects/IdleArcade/Resource/Movement/DampedResourceMovementBehaviour",
    order = 1)]
public class DampedResourceMovementBehaviour : BaseResourceMovementBehaviour
{
    [SerializeField] private float _moveDuration;

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
        float currentTime = 0;

        var producibleTransform = sourceTransform;
        Vector3 position = producibleTransform.position;
        Quaternion rotation = producibleTransform.rotation;
        producibleTransform.SetParent(container);

        while (currentTime < _moveDuration)
        {
            float step = currentTime / _moveDuration;

            if (targetTransform == null)
            {
                yield break;
            }

            producibleTransform.position
                = Vector3.Lerp(
                    position,
                    targetTransform.position,
                    step);

            producibleTransform.rotation = Quaternion.Lerp(rotation, targetTransform.rotation, step);


            currentTime += Time.deltaTime;
            yield return null;
        }

        producibleTransform.position = targetTransform.position;
        producibleTransform.rotation = targetTransform.rotation;


        OnMovementFinished?.Invoke();
    }
}