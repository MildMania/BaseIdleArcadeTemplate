using UnityEngine;

public class MoneyProducer : BaseProducer<Money>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;

    public override Money ProduceCustomActions(Money folder)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(folder.transform);

        Money clonedMoney = ResourcePoolManager.Instance.LoadResource(folder) as Money;
        var moneyTransform = folder.transform;
        clonedMoney.transform.SetPositionAndRotation(moneyTransform.position, moneyTransform.rotation);
        clonedMoney.gameObject.SetActive(true);
        // Folder clonedFolder = Instantiate(folder, folder.transform.position, folder.transform.rotation);

        clonedMoney.Move(targetTransform, _resourceProvider.ResourceContainer);
        return clonedMoney;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveLastTransform();
    }
}