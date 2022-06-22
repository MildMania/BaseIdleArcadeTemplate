public class MoneyConsumer : BaseConsumer<Money>
{
    public override void ConsumeCustomActions(Money money)
    {
        money.GoPoolObject.Push();
    }
}