using UnityEngine;

public class FolderLoadBehaviour : BaseLoadBehaviour<FolderProducer, Folder>
{
    [SerializeField] private FolderProducerFovController _folderProducerFovController;

    protected override void OnAwakeCustomActions()
    {
        base.OnAwakeCustomActions();

        _folderProducerFovController.OnTargetEnteredFieldOfView += OnProducerEnteredFieldOfView;
        _folderProducerFovController.OnTargetExitedFieldOfView += OnProducerExitedFieldOfView;
    }

    protected override void OnDestroyCustomActions()
    {
        base.OnDestroyCustomActions();

        _folderProducerFovController.OnTargetEnteredFieldOfView -= OnProducerEnteredFieldOfView;
        _folderProducerFovController.OnTargetExitedFieldOfView -= OnProducerExitedFieldOfView;
    }

    public override void LoadCustomActions(Folder resource)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(resource.transform);
        resource.Move(targetTransform, _deliverer.Container);
        _deliverer.Resources.Add(resource);
    }
}