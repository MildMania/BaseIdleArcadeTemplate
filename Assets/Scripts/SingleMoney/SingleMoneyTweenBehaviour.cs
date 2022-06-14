using DG.Tweening;
using UnityEngine;

public class SingleMoneyTweenBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _moneyImagePrefab;
    [SerializeField] private Transform _spawnTransform;

    [SerializeField] private float _tweenDuration;

    [SerializeField] private RectTransform _tweenTarget;
    [SerializeField] private RectTransform _tweenArea;

    public RectTransform TweenArea => _tweenArea;


    public void TweenMoney(Vector3 spawnPosition)
    {
        var moneyImageObject = Instantiate(_moneyImagePrefab, _spawnTransform);

        var rectTransform = moneyImageObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = spawnPosition;

        rectTransform.DOAnchorPos(_tweenTarget.anchoredPosition, _tweenDuration).OnComplete(() =>
        {
            Destroy(moneyImageObject);
        });
        
    }


}