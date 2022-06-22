using UnityEngine;

public class MoneyProducer : BaseProducer<Money>
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;
    [SerializeField] private BaseResourceMovementBehaviour _baseResourceMovementBehaviour;

    public override Money ProduceCustomActions(Money money)
    {
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(money.transform);

        Money clonedMoney = ResourcePoolManager.Instance.LoadResource(money) as Money;
        var moneyTransform = money.transform;
        clonedMoney.transform.SetPositionAndRotation(moneyTransform.position, moneyTransform.rotation);
        clonedMoney.gameObject.SetActive(true);
   
        clonedMoney.Move(targetTransform, _resourceProvider.ResourceContainer, Instantiate(_baseResourceMovementBehaviour));
        return clonedMoney;
    }

    protected override void TryRemoveAndGetLastProducibleCustomActions()
    {
        _updatedFormationController.RemoveLastTransform();
    }
}