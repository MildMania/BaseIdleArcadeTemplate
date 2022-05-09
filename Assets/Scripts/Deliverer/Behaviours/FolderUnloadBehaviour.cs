using UnityEngine;

public class FolderUnloadBehaviour : BaseUnloadBehaviour<FolderConsumer, Folder>
{
	[SerializeField] private FolderConsumerFovController _folderConsumerFovController;
	
	protected override void OnAwakeCustomActions()
	{
		base.OnAwakeCustomActions();
		
		_folderConsumerFovController.OnTargetEnteredFieldOfView += OnConsumerEnteredFieldOfView;
		_folderConsumerFovController.OnTargetExitedFieldOfView += OnConsumerExitedFieldOfView;
	}

	protected override void OnDestroyCustomActions()
	{
		base.OnDestroyCustomActions();
		
		_folderConsumerFovController.OnTargetEnteredFieldOfView -= OnConsumerEnteredFieldOfView;
		_folderConsumerFovController.OnTargetExitedFieldOfView -= OnConsumerExitedFieldOfView;
	}

	public override void UnloadCustomActions(int index)
	{
		int lastResourceIndex = _deliverer.GetLastResourceIndex<Folder>();

		if (lastResourceIndex == -1)
		{
			return;
		}
		
		if (_isActiveOnStart)
		{
			_onHapticRequestedEventRaiser.Raise(new OnHapticRequestedEventArgs(_hapticType));
		}
		
		//Remove from self
		Folder folder = (Folder)_deliverer.Resources[lastResourceIndex];
		_deliverer.Resources.Remove(folder);
		_updatedFormationController.RemoveCustomResourceTransform(lastResourceIndex);
		
		//Add To Consumer
		FolderConsumer folderConsumer = _consumers[index];
		UpdatedFormationController consumerFormationController =
			folderConsumer.GetComponentInChildren<UpdatedFormationController>();
		Transform targetTransform = consumerFormationController.GetLastTargetTransform(folder.transform);
		folder.OnMoveRoutineFinished += OnMoveRoutineFinished;
		folder.Move(targetTransform, folderConsumer.ResourceProvider.ResourceContainer);
		folderConsumer.ResourceProvider.Resources.Add(folder);
	}

	private void OnMoveRoutineFinished(IResource folder)
	{
		folder.OnMoveRoutineFinished -= OnMoveRoutineFinished;
		_updatedFormationController.UpdateResourcesPosition(_deliverer.Container);
	}
}