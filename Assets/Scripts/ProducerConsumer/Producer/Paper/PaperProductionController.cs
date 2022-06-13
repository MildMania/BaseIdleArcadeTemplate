public class PaperProductionController : ProductionController<PaperProducer, Paper>
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