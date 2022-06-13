public class FolderProductionController : ProductionController<FolderProducer, Folder>
{
	private void OnEnable()
	{
		StartCoroutine(ProduceRoutine(_resource));
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}