using UnityEngine;

public class PaperProducer : BaseProducer<Paper>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;

    public override Paper ProduceCustomActions(Paper folder)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(folder.transform);

        Paper clonedPaper = Instantiate(folder, folder.transform.position, folder.transform.rotation);

        clonedPaper.Move(targetTransform, _resourceProvider.ResourceContainer);
        return clonedPaper;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveAndGetLastTransform();
    }
}