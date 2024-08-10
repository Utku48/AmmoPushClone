using DG.Tweening;
using TMPro;
using UnityEngine;

public class LvlUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private GameObject _tapToStart;
    [SerializeField] private GameObject _panel;

    [SerializeField] private LvlManager _upgradeManager;

    

    private void Start()
    {
        AnimateBreathing();
    }

    private void Update()
    {
        _moneyCount.text = _upgradeManager.money + "$".ToString();
    }

    private void AnimateBreathing()
    {
        _tapToStart.transform.DOScale(1.1f, 1f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _tapToStart.transform.DOScale(.8f, 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        AnimateBreathing();
                    });
            });
    }

    public void OnPointerClick()
    {
        TimerHandler.Instance.StartTimer();
        _tapToStart.SetActive(false);
        _panel.SetActive(false);

        foreach (var item in _upgradeManager.buttons)
        {
            item.gameObject.SetActive(false);
        }
    }


}
