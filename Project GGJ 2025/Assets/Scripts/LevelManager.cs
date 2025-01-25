using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Components")]
    public RectTransform rollRt;
    public Image beatFiller1;
    public Image beatMarginFiller1;
    public Image beatFiller2;
    public Image beatMarginFiller2;
    public GameObject goodIndicator;
    public GameObject badIndicator;
    public TMP_Text scoreText;
    public TMP_Text performanceText;

    [Header("Parameters")]
    public float beatDuration = 1f;
    public int beatsToAdvance = 6;
    public float goodHitMargin = 0.75f;

    private static LevelManager instance;
    private float currentBeatDuration = 0;
    private int currentBeats = 0;
    private int currentPerformance = 0;
    private int currentScore = 0;

    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        beatMarginFiller1.fillAmount = goodHitMargin;
        beatMarginFiller2.fillAmount = goodHitMargin;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        currentBeatDuration += dt;
        if(currentBeatDuration >= beatDuration)
        {
            currentBeatDuration -= beatDuration;
            currentBeats++;
            if(currentBeats >= beatsToAdvance)
            {
                currentBeats = 0;
                rollRt.anchoredPosition -= new Vector2(0, 100);
                Debug.Log("Adavancing panel");
            }
        }
        beatFiller1.fillAmount = currentBeatDuration / beatDuration;
        beatFiller2.fillAmount = currentBeatDuration / beatDuration;
    }

    public void ExplodeBubble()
    {
        Debug.Log("Bubble exploded");
        float fillAmount = currentBeatDuration / beatDuration;
        if(fillAmount > goodHitMargin)
        {
            Debug.Log("Good");
            StartCoroutine(ActivateAndDeactivate(goodIndicator));
            currentPerformance++;
            currentPerformance = Mathf.Clamp(currentPerformance, -10, 10);
            performanceText.text = "Performance: " + currentPerformance;
            currentScore++;
            scoreText.text = "Score: +" + currentScore;
        }
        else
        {
            Debug.Log("Bad");
            StartCoroutine(ActivateAndDeactivate(badIndicator));
            currentPerformance--;
            currentPerformance = Mathf.Clamp(currentPerformance, -10, 10);
            performanceText.text = "Performance: " + currentPerformance;
            if(currentPerformance <= -10)
            {
                // You lose
            }
        }
    }

    IEnumerator ActivateAndDeactivate(GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
