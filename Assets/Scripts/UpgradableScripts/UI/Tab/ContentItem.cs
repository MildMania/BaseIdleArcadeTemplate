using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentItem : MonoBehaviour
{
    [SerializeField] private ETabCategory _tabCategory;

    public ETabCategory TabCategory => _tabCategory;
}
