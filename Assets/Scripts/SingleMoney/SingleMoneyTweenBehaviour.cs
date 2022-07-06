using DG.Tweening;
using UnityEngine;

public class SingleMoneyTweenBehaviour : MonoBehaviour
{
    [SerializeField] private MoneyLoadBehaviour _moneyLoadBehaviour;

    [SerializeField] private GameObject _moneyImagePrefab;
    [SerializeField] private Transform _spawnTransform;

    [SerializeField] private float _tweenDuration;

    [SerializeField] private RectTransform _tweenTarget;
    [SerializeField] private RectTransform _tweenArea;

    public RectTransform TweenArea => _tweenArea;

    private void Awake()
    {
        if (_moneyLoadBehaviour != null)
        {
            _moneyLoadBehaviour.OnMoneyLoaded += OnMoneyLoaded;
        }
    }

    private void OnDestroy()
    {
        if (_moneyLoadBehaviour != null)
        {
            _moneyLoadBehaviour.OnMoneyLoaded -= OnMoneyLoaded;
        }
    }

    private void OnMoneyLoaded(Money money)
    {
        Vector3 coinScreenPosition =
            GameUtilities.GetWorldToScreenSpace(money.transform.position,
                CameraManager.Instance.MainCamera, _tweenArea);
        TweenMoney(coinScreenPosition);
    }

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