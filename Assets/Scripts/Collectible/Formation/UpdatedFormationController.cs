using System.Collections.Generic;
using UnityEngine;

public class UpdatedFormationController : MonoBehaviour
{
    // [SerializeField] private Transform _container;

    private readonly Dictionary<EFormationGroupType, Transform[]> _formationGroupTypeToLeadingTransforms =
        new Dictionary<EFormationGroupType, Transform[]>();

    private List<Transform>[] _targetTransforms;

    public Vector3 _distance;

    private int _addedTransformCount;

    public List<Transform>[] TargetTransforms
    {
        get => _targetTransforms;
        set => _targetTransforms = value;
    }

    [SerializeField] private EFormationGroupType _currentFormationGroupType;


    public EFormationGroupType CurrentFormationGroupType
    {
        get => _currentFormationGroupType;
        set => _currentFormationGroupType = value;
    }

    public int CurrentColumn { get; set; }

    public int CurrentRow { get; set; }


    private void Awake()
    {
        foreach (var formationGroup in GetComponentsInChildren<FormationGroup>())
        {
            _formationGroupTypeToLeadingTransforms[formationGroup.EFormationGroupType] =
                formationGroup.LeadingTransforms;
        }

        InitTargetTransforms(CurrentFormationGroupType);
    }

    private void InitTargetTransforms(EFormationGroupType eFormationGroupType)
    {
        TargetTransforms =
            new List<Transform>[_formationGroupTypeToLeadingTransforms[eFormationGroupType].Length];
        for (int i = 0; i < TargetTransforms.Length; i++)
        {
            TargetTransforms[i] = new List<Transform>
                {_formationGroupTypeToLeadingTransforms[eFormationGroupType][i]};
        }
    }

    public Transform GetFirstTargetTransform()
    {
        return TargetTransforms[0][0];
    }

    public Transform GetLastTargetTransform(Transform objectTransform)
    {
        //TODO: Update here to work with object pooling
        
        Transform clonedTransform = Instantiate(objectTransform, transform);
        clonedTransform.gameObject.SetActive(false);
        CurrentRow = _addedTransformCount / TargetTransforms.Length;
        CurrentColumn = _addedTransformCount % TargetTransforms.Length;
        clonedTransform.position = TargetTransforms[CurrentColumn][CurrentRow].position + _distance;
        TargetTransforms[CurrentColumn].Add(clonedTransform);
        _addedTransformCount++;
        return clonedTransform;
    }

    public void RemoveLastTransform()
    {
        if (_addedTransformCount == 0)
        {
            return;
        }

        _addedTransformCount--;
        CurrentRow = _addedTransformCount / TargetTransforms.Length;
        CurrentColumn = _addedTransformCount % TargetTransforms.Length;
        Transform targetTransform = TargetTransforms[CurrentColumn][CurrentRow + 1];
        TargetTransforms[CurrentColumn].Remove(targetTransform);

        targetTransform.SetParent(null);
        
        Destroy(targetTransform.gameObject);
    }

    public void RemoveCustomResourceTransform(int index)
    {
        if (_addedTransformCount == 0)
        {
            return;
        }

        _addedTransformCount--;
        CurrentRow = index / TargetTransforms.Length;
        CurrentColumn = index % TargetTransforms.Length;
        UpdateFormationTransforms(index);
        Transform targetTransform = TargetTransforms[CurrentColumn][CurrentRow + 1];
        TargetTransforms[CurrentColumn].Remove(targetTransform);
        targetTransform.SetParent(null);
        Destroy(targetTransform.gameObject);
    }

    public void UpdateResourcesPosition(Transform container)
    {
        var resources = container.GetComponentsInChildren<Transform>();
        int i = 0;
        foreach (var resource in resources)
        {
            resource.position = TargetTransforms[0][i].position;
            i++;
        }
    }

    public void UpdateFormationTransforms(int index)
    {
        int maxRow = _addedTransformCount / TargetTransforms.Length;

        var currentRow = index / TargetTransforms.Length;
        var currentColumn = index % TargetTransforms.Length;

        var targetPosition = TargetTransforms[currentColumn][currentRow + 1].position;
        while (index < maxRow)
        {
            currentRow = index / TargetTransforms.Length;
            currentColumn = index % TargetTransforms.Length;
            var temp = TargetTransforms[currentColumn][currentRow + 2].position;
            TargetTransforms[currentColumn][currentRow + 2].position = targetPosition;
            targetPosition = temp;
            index++;
        }
    }
}