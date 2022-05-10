public interface IConsumer<TResource> where TResource : BaseResource
{
    public void Consume(TResource resource);
}