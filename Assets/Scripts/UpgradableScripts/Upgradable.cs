﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class Upgradable : SerializedMonoBehaviour
{
	[OdinSerialize] protected EUpgradable _upgradableType;

	protected IRequirement[] RequirementData
		= Array.Empty<IRequirement>();

	public Action<UpgradableTrackData> OnUpgraded;

	protected UpgradableTrackData _upgradableTrackData;

	private void Awake()
	{
		List<IRequirement> reqList = GameConfigManager.Instance.CreateRequirementList(EAttributeCategory.CHARACTER, _upgradableType);

		if (reqList == null)
		{
			return;
		}
		RequirementData = reqList.ToArray();
	}

	private void Start()
	{
		UpgradableTrackable upgradableTrackable;

		if (UserManager.Instance.LocalUser.GetUserData<UserUpgradableData>().Tracker
			.TryGetSingle(_upgradableType, out upgradableTrackable))
		{
			_upgradableTrackData = upgradableTrackable.TrackData;
		}
		else
		{
			_upgradableTrackData = new UpgradableTrackData(_upgradableType, 0);
			upgradableTrackable = new UpgradableTrackable(_upgradableTrackData);
			UserManager.Instance.LocalUser.GetUserData<UserUpgradableData>().Tracker.TryCreate(upgradableTrackable);
		} 
		
		OnUpgraded?.Invoke(_upgradableTrackData);
	}

	public bool TryUpgrade(User user, IRequirement requirementData)
	{
		return RequirementUtilities.TrySatisfyRequirements(
			user, new []{ requirementData });
	}
	

}