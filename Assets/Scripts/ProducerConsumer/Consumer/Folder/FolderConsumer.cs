using UnityEngine;

public class FolderConsumer : BaseConsumer<Folder>
{
    public override void ConsumeCustomActions(Folder folder)
    {
        Transform targetTransform = _updatedFormationController.GetFirstTargetTransform();
        folder.Move(targetTransform, null, Instantiate(_baseResourceMovementBehaviour));
        
        folder.OnMovementFinished += OnMoveRoutineFinished;

    }

    private void OnMoveRoutineFinished(BaseResource resource)
    {
        Folder folder = (Folder) resource;
        folder.OnMovementFinished -= OnMoveRoutineFinished;
        _onMoveFinishedTaskExecutor?.Execute(this);
        folder.GoPoolObject.Push();
    
        OnConsumeFinished?.Invoke(this, folder);
        OnConsumed?.Invoke();
    }
}