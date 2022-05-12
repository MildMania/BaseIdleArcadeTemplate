using UnityEngine;

public class PaperProducer : BaseProducer<Paper>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;

    public override Paper ProduceCustomActions(Paper money)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(money.transform);

        Paper clonedPaper = (Paper) ResourcePoolManager.Instance.LoadResource(money);
        var paperTransform = money.transform;
        clonedPaper.transform.SetPositionAndRotation(paperTransform.position, paperTransform.rotation);
        clonedPaper.gameObject.SetActive(true);
        // Paper clonedPaper = Instantiate(folder, folder.transform.position, folder.transform.rotation);

        clonedPaper.Move(targetTransform, _resourceProvider.ResourceContainer);
        return clonedPaper;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveLastTransform();
    }
}