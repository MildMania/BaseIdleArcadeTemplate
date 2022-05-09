using System;
using System.Collections.Generic;
using UnityEngine;

public class PaperToMoneyRequirement : BaseRequirement
{
    [SerializeField] private int _amountNeeded;
    [SerializeField] private PaperProvider _paperProvider;
    [SerializeField] private PaperConsumptionController _paperConsumptionController;

    public override bool IsRequirementMet()
    {
        List<Paper> papers = _paperProvider.Resources;
        if (!_paperConsumptionController.IsAvailable || papers == null || papers.Count < _amountNeeded)
        {
            return false;
        }

        return true;
    }

    public override void ExecuteRequirement(Action onRequirementExecuted)
    {
        _paperConsumptionController.StartConsumption(_amountNeeded, onRequirementExecuted);
    }
}