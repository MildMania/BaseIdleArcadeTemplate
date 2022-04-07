using UnityEngine;

public class FolderProductionController : ProductionController<FolderProducer, Folder>
{
	// [SerializeField] private GameObject _maxIndicatorObject;

	[SerializeField] private ProduceCapacityRequirement _capacityRequirement;
	private void OnEnable()
	{
		StartCoroutine(ProduceRoutine(_resource));
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
	
	// private void Update()
	// {
	// 	_maxIndicatorObject.SetActive(!_capacityRequirement.IsProductionRequirementMet());
	// }
}