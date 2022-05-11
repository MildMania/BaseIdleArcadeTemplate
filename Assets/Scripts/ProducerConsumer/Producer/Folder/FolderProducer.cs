using UnityEngine;

public class FolderProducer : BaseProducer<Folder>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;

    public override Folder ProduceCustomActions(Folder folder)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(folder.transform);

        Folder clonedFolder = (Folder) ResourcePoolManager.Instance.LoadResource(folder);
        var folderTransform = folder.transform;
        clonedFolder.transform.SetPositionAndRotation(folderTransform.position, folderTransform.rotation);
        clonedFolder.gameObject.SetActive(true);
        // Folder clonedFolder = Instantiate(folder, folder.transform.position, folder.transform.rotation);

        clonedFolder.Move(targetTransform, _resourceProvider.ResourceContainer);
        return clonedFolder;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveLastTransform();
    }
}