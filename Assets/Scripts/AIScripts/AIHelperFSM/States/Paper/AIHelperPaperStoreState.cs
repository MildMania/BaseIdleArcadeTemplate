using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EState = AIHelperFSMController.EState;
using ETransition = AIHelperFSMController.ETransition;

public class AIHelperPaperStoreState : AIHelperStoreState
{
    [SerializeField] private PaperDelivererAIHelper _paperDelivererAiHelper;

    // Do I need to create a new list every time this method is called?
    protected override List<IAIInteractable> GetProducers()
    {
        return ProducerProvider.Instance.GetProducers(typeof(Paper));
    }

    protected override void OnStoreStateCustomActions()
    {
        _paperDelivererAiHelper.PaperLoadBehaviour.OnCapacityFull += OnCapacityFull;
    }

    protected override void OnExitCustomActions()
    {
        base.OnExitCustomActions();

        _paperDelivererAiHelper.PaperLoadBehaviour.OnCapacityFull -= OnCapacityFull;
    }

    private void OnCapacityFull()
    {
        FSM.SetTransition(ETransition.Deliver);
    }
}
