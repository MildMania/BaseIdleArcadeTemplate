﻿using System;
using System.Collections.Generic;

public class UpgradableUpdater
{
	private IUpgradableDataProvider _upgradableDataProvider;

	public Action<EUpgradable, int> OnUpgradableUpdated { get; set; }
    
	public UpgradableUpdater(
		IUpgradableDataProvider upgradableDataProvider)
	{
		_upgradableDataProvider = upgradableDataProvider;
	}

	public void UpdateUpgradable(EUpgradable upgradable, int level, Dictionary<string,string> attributes)
	{
		_upgradableDataProvider.SetUpgradable(upgradable, 
			_upgradableDataProvider.GetUpgradableLevel(upgradable) + level,attributes);

		OnUpgradableUpdated?.Invoke(upgradable, _upgradableDataProvider.GetUpgradableLevel(upgradable));
	}

	public int GetUpgradable(EUpgradable upgradable)
	{
		return _upgradableDataProvider.GetUpgradableLevel(upgradable);
	}
}