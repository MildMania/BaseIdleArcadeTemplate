using UnityEngine;

public class PaperProducer : BaseProducer<Paper>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;

    public override Paper ProduceCustomActions(Paper paper)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(paper.transform);

        Paper clonedPaper = (Paper) ResourcePoolManager.Instance.LoadResource(paper);
        var paperTransform = paper.transform;
        clonedPaper.transform.SetPositionAndRotation(paperTransform.position, paperTransform.rotation);
        clonedPaper.gameObject.SetActive(true);

        clonedPaper.ProduceMovementBehaviour.Move(targetTransform, _resourceProvider.ResourceContainer);

        return clonedPaper;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveLastTransform();
    }
}