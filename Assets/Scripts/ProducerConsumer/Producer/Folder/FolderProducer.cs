using UnityEngine;

public class FolderProducer : BaseProducer<Folder>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;
    [SerializeField] private BaseResourceMovementBehaviour _baseResourceMovementBehaviour;
    public override Folder ProduceCustomActions(Folder folder)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(folder.transform);

        Folder clonedFolder = (Folder) ResourcePoolManager.Instance.LoadResource(folder);
        var folderTransform = folder.transform;
        clonedFolder.transform.SetPositionAndRotation(folderTransform.position, folderTransform.rotation);
        clonedFolder.gameObject.SetActive(true);
    
        clonedFolder.Move(targetTransform, _resourceProvider.ResourceContainer, Instantiate(_baseResourceMovementBehaviour));
        return clonedFolder;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveLastTransform();
    }
}