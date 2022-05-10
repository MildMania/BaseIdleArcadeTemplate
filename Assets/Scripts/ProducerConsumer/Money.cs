using System.Collections;
using UnityEngine;

public class Money :  BaseResource
{
    private float _moveDuration = 0.05f;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public override void Move(Transform target, Transform container)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(target, container));
    }

    private IEnumerator MoveRoutine(Transform target, Transform container)
    {
        float currentTime = 0;

        var producibleTransform = transform;
        Vector3 position = producibleTransform.position;
        Quaternion rotation = producibleTransform.rotation;
        producibleTransform.SetParent(container);

        while (currentTime < _moveDuration)
        {
            float step = currentTime / _moveDuration;

            producibleTransform.position = Vector3.Lerp(position,
                target.position, step);
            producibleTransform.rotation = Quaternion.Lerp(rotation, target.rotation, step);


            currentTime += Time.deltaTime;
            yield return null;
        }

        producibleTransform.position = target.position;
        producibleTransform.rotation = target.rotation;

        OnMoveRoutineFinished?.Invoke(this);
    }
}