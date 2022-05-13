using System;
using System.Collections.Generic;
using MMFramework.TasksV2;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class Upgradable : SerializedMonoBehaviour
{
    [OdinSerialize] protected EUpgradable _upgradableType;
    [OdinSerialize] private EAttributeCategory _attributeCategory;
    [OdinSerialize] private MMTaskExecutor _onUpgradedTasks;

    private CoinController _coinController;

    protected IRequirement[] RequirementData
        = Array.Empty<IRequirement>();

    public Action<UpgradableTrackData> OnUpgraded;

    protected UpgradableTrackData _upgradableTrackData;

    public UpgradableTrackData UpgradableTrackData => _upgradableTrackData;

    public EAttributeCategory AttributeCategory => _attributeCategory;
    public EUpgradable UpgradableType => _upgradableType;


    private void Awake()
    {
        List<IRequirement> reqList =
            GameConfigManager.Instance.CreateRequirementList(_attributeCategory, _upgradableType);

        if (reqList == null)
        {
            return;
        }

        RequirementData = reqList.ToArray();

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
        UpgradableManager.Instance.CoinController.UpdateCoinCount();
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

    private bool IsSatisfyRequirements(User user, IRequirement requirementData)
    {
        return RequirementUtilities.TrySatisfyRequirements(
            user, new[] {requirementData});
    }

    public bool TryUpgrade()
    {
        if (_upgradableTrackData.Level > RequirementData.Length - 1)
        {
            return false;
        }

        if (IsSatisfyRequirements(UserManager.Instance.LocalUser, RequirementData[_upgradableTrackData.Level]))
        {
            Debug.Log("UPGRADE FAILED");

            var requirementCoin = (RequirementCoin) RequirementData[_upgradableTrackData.Level];
            int totalRequiredAmount = requirementCoin.RequirementData.RequiredAmount;

            UpgradableTrackData upgradableTrackData = new UpgradableTrackData(_upgradableTrackData.TrackID,
                ++_upgradableTrackData.Level);

            var userUpgradableData = UserManager.Instance.LocalUser.GetUserData<UserUpgradableData>();
            userUpgradableData.Tracker.TryUpsert(upgradableTrackData);

            var userCoinInventoryData = UserManager.Instance.LocalUser.GetUserData<UserCoinInventoryData>();
            Coin trackableCoin;
            userCoinInventoryData.Tracker.TryGetSingle(ECoin.Gold, out trackableCoin);

            CoinTrackData coinTrackData =
                new CoinTrackData(
                    ECoin.Gold,
                    count: trackableCoin.TrackData.CurrentCount - totalRequiredAmount);

            trackableCoin.UpdateData(coinTrackData);
            UserManager.Instance.LocalUser.SaveData(OnSaved);

            void OnSaved()
            {
                Debug.Log("upgrade saved");
            }

            Debug.Log("UPGRADE YAPILDI.");

            OnUpgraded?.Invoke(_upgradableTrackData);
            UpgradableManager.Instance.CoinController.UpdateCoinCount();
            _onUpgradedTasks?.Execute(this);

            return true;
        }

        return false;
    }
}