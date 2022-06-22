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

    public override void LoadCustomActions(Folder folder)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(folder.transform);
        
        folder.Move(targetTransform, _deliverer.Container, Instantiate(_baseResourceMovementBehaviour));
        _deliverer.Resources.Add(folder);
    }
}