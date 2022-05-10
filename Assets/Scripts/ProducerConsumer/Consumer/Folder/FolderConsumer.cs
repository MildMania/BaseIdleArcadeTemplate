using UnityEngine;

public class FolderConsumer : BaseConsumer<Folder>
{
    public override void ConsumeCustomActions(Folder folder)
    {
        Transform targetTransform = _updatedFormationController.GetFirstTargetTransform();
        folder.Move(targetTransform, null);
        folder.OnMoveRoutineFinished += OnMoveRoutineFinished;
        Debug.Log("FOLDER CONSUMED");
    }

    private void OnMoveRoutineFinished(BaseResource baseResource)
    {
        Folder folder = (Folder) baseResource;
        folder.OnMoveRoutineFinished -= OnMoveRoutineFinished;
        folder.GOPoolObject.Push();
        // folder.gameObject.SetActive(false);
        OnConsumeFinished?.Invoke(this, folder);
    }
}