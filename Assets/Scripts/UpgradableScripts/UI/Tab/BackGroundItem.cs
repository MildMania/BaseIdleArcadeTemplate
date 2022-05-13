using UnityEngine;


public class BackGroundItem : MonoBehaviour
{
	[SerializeField] private ETabCategory _tabCategory;

	public ETabCategory TabCategory => _tabCategory;
}