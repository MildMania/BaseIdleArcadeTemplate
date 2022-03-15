using UnityEngine;

public class StorableCarrierStoreHandler : StorableStoreHandler
{
	[SerializeField] private BaseProducerDetector _baseProducerDetector;


	private void Awake()
	{
		_baseProducerDetector.OnDetected += OnProducerDetected;
		_baseProducerDetector.OnEnded += OnProducerEnded;
	}


	private void OnDestroy()
	{
		_baseProducerDetector.OnDetected -= OnProducerDetected;
		_baseProducerDetector.OnEnded -= OnProducerEnded;
	}

	private void OnProducerDetected(ProducerBase producerBase)
	{
		var storableDropHandler = producerBase.GetComponentInChildren<StorableDropHandler>();

		storableDropHandler.OnStorableDropped += OnStorableDropped;
		storableDropHandler.StartDrop();
	}

	private void OnStorableDropped(StorableBase storableBase)
	{
		_storableFormationController.Reformat();
		StoreStorable(storableBase);
	}
	

	private void OnProducerEnded(ProducerBase producerBase)
	{
		var storableDropHandler = producerBase.GetComponentInChildren<StorableDropHandler>();

		storableDropHandler.StopDrop();
		storableDropHandler.OnStorableDropped -= OnStorableDropped;
	}
}