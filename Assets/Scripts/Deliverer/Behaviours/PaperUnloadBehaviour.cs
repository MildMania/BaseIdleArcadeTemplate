using System;
using UnityEngine;

public class PaperUnloadBehaviour : BaseUnloadBehaviour<PaperConsumer, Paper>
{
    [SerializeField] private PaperConsumerFovController _paperConsumerFovController;
    
    private bool _isFirstUnload = false;
    public Action OnFistUnloaded;

    protected override void OnAwakeCustomActions()
    {
        base.OnAwakeCustomActions();

        _paperConsumerFovController.OnTargetEnteredFieldOfView += OnConsumerEnteredFieldOfView;
        _paperConsumerFovController.OnTargetExitedFieldOfView += OnConsumerExitedFieldOfView;
    }

    protected override void OnDestroyCustomActions()
    {
        base.OnDestroyCustomActions();

        _paperConsumerFovController.OnTargetEnteredFieldOfView -= OnConsumerEnteredFieldOfView;
        _paperConsumerFovController.OnTargetExitedFieldOfView -= OnConsumerExitedFieldOfView;
    }

    public override void UnloadCustomActions(int index)
    {
        if (!_isFirstUnload)
        {
            _isFirstUnload = true;
            OnFistUnloaded?.Invoke();
        }
        //Remove from self
        int lastResourceIndex = _deliverer.GetLastResourceIndex<Paper>();

        if (lastResourceIndex == -1)
        {
            return;
        }
        
        if (_isActiveOnStart)
        {
            _onHapticRequestedEventRaiser.Raise(new OnHapticRequestedEventArgs(_hapticType));
        }
        
        Paper paper = (Paper) _deliverer.Resources[lastResourceIndex];
        _deliverer.Resources.Remove(paper);
        _updatedFormationController.RemoveCustomResourceTransform(lastResourceIndex);

        //Add To Consumer
        PaperConsumer paperConsumer = _consumers[index];
        UpdatedFormationController consumerFormationController =
            paperConsumer.GetComponentInChildren<UpdatedFormationController>();
        Transform targetTransform = consumerFormationController.GetLastTargetTransform(paper.transform);

        paper.UnloadMovementBehaviour.OnMoveRoutineFinished += OnMoveRoutineFinished;
        paper.UnloadMovementBehaviour.Move(targetTransform, paperConsumer.ResourceProvider.ResourceContainer);
        

        paperConsumer.AddToResourceProvider(paper);
    }

    private void OnMoveRoutineFinished(BaseResource paper)
    {
        paper.UnloadMovementBehaviour.OnMoveRoutineFinished -= OnMoveRoutineFinished;
        
        _updatedFormationController.UpdateResourcesPosition(_deliverer.Container);
    }
}