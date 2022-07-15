using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMFramework.TasksV2;
using EState = AIHelperFSMController.EState;
using ETransition = AIHelperFSMController.ETransition;
using Pathfinding;
using Pathfinding.RVO;
using DG.Tweening;

public class AIHelperLoadState : State<EState, ETransition>
{
    [SerializeField] private RVOController _rvoController;
    [SerializeField] private AIHelper _aiHelper;
    [SerializeField] protected AIMovementBehaviour _movementBehaviour;
    [SerializeField] protected MMTaskExecutor _onMovementCompletedTasks;
    [SerializeField] private HelperAnimationController _helperAnimationController;
    [SerializeField] private float _pollDelay = 5;

    private BaseProducer _currentProducer;
    private WaitForSeconds _pollWfs;

    private void Awake()
    {
        _pollWfs = new WaitForSeconds(_pollDelay);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected override EState GetStateID()
    {
        return EState.Load;
    }

    public override void OnEnterCustomActions()
    {
        _aiHelper.PickRandomLoadUnloadBehaviour(() => StartCoroutine(SelectProducerRoutine()));
    }

    protected override void OnExitCustomActions()
    {
        _aiHelper.CurrentLoadBehaviour.OnCapacityFull -= OnCapacityFull;

        // _rvoController.locked = false;

        _aiHelper.CurrentLoadBehaviour.Deactivate();

        // _aiHelper.ReleaseProducer(_currentProducer);
    }

    private IEnumerator SelectProducerRoutine()
    {
        _currentProducer = SelectProducer();

        while (_currentProducer == null)
        {
            yield return _pollWfs;
            _currentProducer = SelectProducer();
        }

        MoveToStorePoint();
    }

    private void MoveToStorePoint()
    {
        MoveToInteractionPoint(_currentProducer.AiInteraction.GetInteractionPoint());
        _helperAnimationController.PlayAnimation(EHelperAnimation.Walk);

        _aiHelper.CurrentLoadBehaviour.OnCapacityFull += OnCapacityFull;
    }


    private BaseProducer SelectProducer()
    {
        List<BaseProducer> allProducers = GetProducers();

        if (allProducers == null || allProducers.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, allProducers.Count);
        var currentProducer = allProducers[index];

        return currentProducer;
    }

    protected List<BaseProducer> GetProducers()
    {
        return ProducerProvider.Instance.GetProducers(_aiHelper.Resource.GetType());
    }

    private void MoveToInteractionPoint(Vector3 pos)
    {
        _movementBehaviour.MoveDestination(pos, OnPathCompleted, OnPathStucked);
    }

    private void OnPathCompleted()
    {
        // _rvoController.locked = true;

        Vector3 rotTarget = _currentProducer.AiInteraction.RotationTarget.position;

        Vector3 dir = (new Vector3(rotTarget.x, _aiHelper.transform.position.y, rotTarget.z) -
                       _aiHelper.transform.position).normalized;

        _aiHelper.transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.1f);


        _aiHelper.CurrentLoadBehaviour.Activate();

        if (_onMovementCompletedTasks != null)
            _onMovementCompletedTasks.Execute(this);

        _helperAnimationController.PlayAnimation(EHelperAnimation.Idle);
    }

    private void OnCapacityFull()
    {
        _aiHelper.CurrentLoadBehaviour.Deactivate();
        FSM.SetTransition(ETransition.Unload);
    }

    private void OnPathStucked()
    {
        Logger.Log("STOPPED AT STORE");
        _movementBehaviour.Stop();

        FSM.SetTransition(ETransition.Unload);
    }
}