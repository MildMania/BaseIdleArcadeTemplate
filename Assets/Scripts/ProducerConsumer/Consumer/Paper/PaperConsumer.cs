using System;
using UnityEngine;

public class PaperConsumer : BaseConsumer<Paper>
{
    public override void ConsumeCustomActions(Paper paper)
    {
        Transform targetTransform = _updatedFormationController.GetFirstTargetTransform();
        
        paper.ConsumeMovementBehaviour.Move(targetTransform, null);
        paper.ConsumeMovementBehaviour.OnMoveRoutineFinished += OnMoveRoutineFinished;

    }

    private void OnMoveRoutineFinished(BaseResource baseResource)
    {
        Paper paper = (Paper) baseResource;
        paper.ConsumeMovementBehaviour.OnMoveRoutineFinished -= OnMoveRoutineFinished;
        _onMoveFinishedTaskExecutor?.Execute(this);
        paper.GOPoolObject.Push();

        OnConsumeFinished?.Invoke(this, paper);
        OnConsumed?.Invoke();
    }
}