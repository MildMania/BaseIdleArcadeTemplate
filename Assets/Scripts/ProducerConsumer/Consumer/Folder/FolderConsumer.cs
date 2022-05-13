using UnityEngine;

public class FolderConsumer : BaseConsumer<Folder>
{
    public override void ConsumeCustomActions(Folder folder)
    {
        Transform targetTransform = _updatedFormationController.GetFirstTargetTransform();
        folder.ConsumeMovementBehaviour.Move(targetTransform, null);
        
        folder.ConsumeMovementBehaviour.OnMoveRoutineFinished += OnMoveRoutineFinished;

    }

    private void OnMoveRoutineFinished(BaseResource resource)
    {
        Folder folder = (Folder) resource;
        folder.ConsumeMovementBehaviour.OnMoveRoutineFinished -= OnMoveRoutineFinished;
        folder.GOPoolObject.Push();
    
        OnConsumeFinished?.Invoke(this, folder);
    }
}