using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerHandler : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;


    [SerializeField] private Image fillImage;
    [SerializeField] public TextMeshProUGUI timer_Text;
    public int duration;
    [SerializeField]
    private Image _timeOutImage;

    private int remainingDuration; // kalan saniye

    public static TimerHandler Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        timer_Text.text = duration.ToString();
    }
    public void StartTimer()
    {
        Vector3 initialPosition = gameObject.transform.position; // İlk pozisyonu kaydet


        gameObject.transform
            .DOMove(new Vector3(initialPosition.x, initialPosition.y - 200, initialPosition.z), .5f)
            .OnComplete(() =>
            {
                gameObject.transform
                    .DOScale(new Vector3(4f, 4f, 4f), .5f)
                    .OnComplete(() =>
                    {
                        gameObject.transform
                                    .DOScale(new Vector3(2f, 2f, 2f), .5f)
                                    .OnComplete(() =>
                                    {
                                        gameObject.transform
                            .DOMove(initialPosition, .5f)
                            .OnComplete(() =>
                            {

                                remainingDuration = duration;
                                StartCoroutine(nameof(TimerCoroutine));
                            });
                                    });
                    });
            });
    }


    private IEnumerator TimerCoroutine()
    {
        while (remainingDuration >= 0)
        {
            timer_Text.text = $"{remainingDuration % 60}";
            fillImage.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);
            remainingDuration -= 1;
            yield return new WaitForSeconds(1f);
        }

        OnTimerEnd();
    }

    private void OnTimerEnd()
    {
        _timeOutImage.transform.DOScale(new Vector3(11f, 11f, 11), 0.75f);
        StartCoroutine(CameraShake());

    }

    private IEnumerator CameraShake()
    {
        yield return new WaitForSeconds(1);
        _timeOutImage.transform.GetChild(0).gameObject.SetActive(true);

        float shakeDuration = 0.3f;
        float shakeAmplitude = .5f;
        float shakeFrequency = 7f;


        var perlinNoise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (perlinNoise != null)
        {
            perlinNoise.m_AmplitudeGain = shakeAmplitude;
            perlinNoise.m_FrequencyGain = shakeFrequency;


            yield return new WaitForSeconds(shakeDuration);


            perlinNoise.m_AmplitudeGain = 0f;
            perlinNoise.m_FrequencyGain = 0f;

            SceneManager.LoadScene(1);
        }

    }
}
