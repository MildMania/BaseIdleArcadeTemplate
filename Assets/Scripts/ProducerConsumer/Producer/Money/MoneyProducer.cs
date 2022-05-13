using UnityEngine;

public class MoneyProducer : BaseProducer<Money>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;

    public override Money ProduceCustomActions(Money money)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(money.transform);

        Money clonedMoney = ResourcePoolManager.Instance.LoadResource(money) as Money;
        var moneyTransform = money.transform;
        clonedMoney.transform.SetPositionAndRotation(moneyTransform.position, moneyTransform.rotation);
        clonedMoney.gameObject.SetActive(true);
   
        clonedMoney.ProduceMovementBehaviour.Move(targetTransform, _resourceProvider.ResourceContainer);
        return clonedMoney;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveLastTransform();
    }
}