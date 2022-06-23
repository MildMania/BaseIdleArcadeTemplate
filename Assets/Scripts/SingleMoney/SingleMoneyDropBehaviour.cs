using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SingleMoneyDropBehaviour : MonoBehaviour
{
	[SerializeField] private Transform _targetTransform;
	[SerializeField] private SingleMoney _singleMoneyPrefab;

	[Header("Targets")] [SerializeField] private Transform _moneySpawnPos;
	[SerializeField] private Transform _moneyDropPos;

	[SerializeField] private float _dropRadiusCoefficient;
	[SerializeField] private int _dropAmount;

	[SerializeField] private float _dropDuration;

	private void Start()
	{
		DropMoneys();
	}

	private GameObject CreateSingleMoneyObject()
	{
		//var singleMoneyObject = Instantiate(_singleMoneyPrefab);
		GameObject singleMoneyObject = ResourcePoolManager.Instance.LoadResource(_singleMoneyPrefab).gameObject;
		singleMoneyObject.SetActive(true);

		singleMoneyObject.transform.position = _moneySpawnPos.transform.position;
		int randomRotation = Random.Range(0, 36) * 10;
		singleMoneyObject.transform.Rotate(Vector3.up, randomRotation);

		return singleMoneyObject;
	}


	private void DropSingleMoney()
	{
		var singleMoneyObject = CreateSingleMoneyObject();

		Vector3 randomPoint = Random.insideUnitSphere * _dropRadiusCoefficient;

		randomPoint.y = _moneyDropPos.transform.position.y;

		var audiencePosition = _targetTransform.position;

		Vector3 dropPoint = new Vector3(audiencePosition.x + randomPoint.x, randomPoint.y,
			audiencePosition.z + randomPoint.z);

		singleMoneyObject.transform.DOJump(dropPoint, 1, 1, _dropDuration);
	}

	public void DropMoneys()
	{
		for (int i = 0; i < _dropAmount; i++)
		{
			DropSingleMoney();
		}
	}
}