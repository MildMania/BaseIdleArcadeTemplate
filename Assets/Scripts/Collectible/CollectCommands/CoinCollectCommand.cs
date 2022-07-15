using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinCollectCommand",
	menuName = "ScriptableObjects/CoinCollectCommand",
	order = 1)]
public class CoinCollectCommand : BaseCollectCommand
{
	protected override void ExecuteCustomActions(Collectible collectible, Action onUncollectCommandExecuted)
	{
		CoroutineRunner.Instance.StartCoroutine(MoveRoutine(collectible));
	}

	private IEnumerator MoveRoutine(Collectible collectible)
	{
		Vector3 distance = Vector3.zero;
		do
		{
			Vector3 CharacterPosition = CharacterTransform.position;
			Vector3 collectiblePosition = collectible.transform.position;

			distance = CharacterPosition - collectiblePosition;

			collectible.transform.position =
				Vector3.MoveTowards(collectiblePosition,
					CharacterPosition,
					Time.deltaTime * 20f);

			Quaternion targetRotation =
				Quaternion.LookRotation(distance);

			collectible.transform.rotation =
				Quaternion.RotateTowards(collectible.transform.rotation, targetRotation, Time.deltaTime * 400f);
			yield return null;
		} while (distance.magnitude > 0.05f);
		
		// Destroy(collectible.gameObject);
		collectible.gameObject.SetActive(false);
		OnCollectCommandFinished?.Invoke();
		Logger.Log("MoveRoutineEND");
		yield return null;
	}
}