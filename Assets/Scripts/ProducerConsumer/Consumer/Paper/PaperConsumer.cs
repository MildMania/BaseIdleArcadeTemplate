using System;
using UnityEngine;

public class PaperConsumer : BaseConsumer<Paper>
{
    public override void ConsumeCustomActions(Paper paper)
    {
        Transform targetTransform = _updatedFormationController.GetFirstTargetTransform();

        paper.Move(targetTransform, null, Instantiate(_baseResourceMovementBehaviour));
        paper.OnMovementFinished += OnMoveRoutineFinished;
    }

    private void OnMoveRoutineFinished(BaseResource baseResource)
    {
        Paper paper = (Paper) baseResource;
        paper.OnMovementFinished -= OnMoveRoutineFinished;
        _onMoveFinishedTaskExecutor?.Execute(this);
        paper.GoPoolObject.Push();

        OnConsumeFinished?.Invoke(this, paper);
        OnConsumed?.Invoke();
    }
}