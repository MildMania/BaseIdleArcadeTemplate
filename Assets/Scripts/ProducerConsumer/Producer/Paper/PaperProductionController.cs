using System;
using MMFramework_2._0.PhaseSystem.Core.EventListener;
using UnityEngine;

public class PaperProductionController : ProductionController<PaperProducer, Paper>
{
	// [SerializeField] private GameObject _maxIndicatorObject;
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
	// 	_maxIndicatorObject.SetActive(!IsAllRequirementMet);
	// }
}