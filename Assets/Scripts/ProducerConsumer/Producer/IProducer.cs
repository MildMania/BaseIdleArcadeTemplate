public interface IProducer<TResource> where TResource : BaseResource
{
    public void Produce(TResource resource);
}