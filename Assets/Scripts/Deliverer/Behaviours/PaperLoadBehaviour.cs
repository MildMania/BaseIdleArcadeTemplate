using System;
using UnityEngine;

public class PaperLoadBehaviour : BaseLoadBehaviour<PaperProducer, Paper>
{
    [SerializeField] private PaperProducerFovController _paperProducerFovController;

    private bool _isFirstLoad = false;
    public Action OnFistLoaded;
    protected override void OnAwakeCustomActions()
    {
        base.OnAwakeCustomActions();

        _paperProducerFovController.OnTargetEnteredFieldOfView += OnProducerEnteredFieldOfView;
        _paperProducerFovController.OnTargetExitedFieldOfView += OnProducerExitedFieldOfView;
    }

    protected override void OnDestroyCustomActions()
    {
        base.OnDestroyCustomActions();

        _paperProducerFovController.OnTargetEnteredFieldOfView -= OnProducerEnteredFieldOfView;
        _paperProducerFovController.OnTargetExitedFieldOfView -= OnProducerExitedFieldOfView;
    }

    public override void LoadCustomActions(Paper resource)
    {
        if (!_isFirstLoad)
        {
            _isFirstLoad = true;
            OnFistLoaded?.Invoke();
        }
        Transform targetTransform = _updatedFormationController.GetLastTargetTransform(resource.transform);
        resource.Move(targetTransform, _deliverer.Container);
        _deliverer.Resources.Add(resource);
    }
}