namespace WarHeroes.InventorySystem
{
    public class InitializeInventoryItemAction_Equip : InitializeInventoryItemActionBase
    {
        public override void Execute<TTracker, TTrackable, TTrackInfo, TID, TEnum>(
            InventoryBase<TTracker, TTrackable, TTrackInfo, TID, TEnum> inventory,
            TTrackable trackable)
        {
            IEquipper equipper = inventory as IEquipper;

            if (equipper == null)
                return;

            IEquippable equippable = trackable as IEquippable;

            if (equippable == null
                || !equippable.Equippable.IsEquipped)
                return;

            //Logger.Log("Equippable: " + trackable.TrackData.TrackID.ToString());

            equipper.Equipper.TryEquip(equippable);
        }
    }
}
