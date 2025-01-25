using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Components")]
    public GameObject catfish;
    Animator catfishAnimator;
    [SerializeField] float timeAngry = 5f;
    float currentTimeAngry;
    [SerializeField] float timeHappy = 5f;
    float currentTimeHappy;
    // Dictionary<string, float> statesDurations;

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
    private FMODUnity.StudioEventEmitter musicEventEmitter;
    private FMODUnity.StudioEventEmitter bubbleEventEmitter;
    private float currentBeatDuration = 0;
    private int currentBeats = 0;
    private int currentPerformance = 0;
    private int currentScore = 0;
    private int currentMusicIntensity = 0;

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
        catfishAnimator = catfish.GetComponent<Animator>();
        // statesDurations = new() {
        //     {"Angry", timeAngry},
        //     {"Happy", timeHappy}
        // };
        currentTimeAngry = timeAngry;
        currentTimeHappy = timeHappy;

        musicEventEmitter = GameObject.Find("Music").GetComponent<FMODUnity.StudioEventEmitter>();
        bubbleEventEmitter = GameObject.Find("Bubble Effects").GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        currentBeatDuration += dt;
        if(currentBeatDuration >= beatDuration)
        {
            //currentBeatDuration -= beatDuration;
            currentBeatDuration = 0;
            musicEventEmitter.Stop();
            musicEventEmitter.Play();
            musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            currentBeats++;
            if(currentBeats >= beatsToAdvance)
            {
                currentBeats = 0;
                rollRt.anchoredPosition -= new Vector2(0, 100);
                // Debug.Log("Adavancing panel");
            }
        }
        // Barras de beat
        beatFiller1.fillAmount = currentBeatDuration / beatDuration;
        beatFiller2.fillAmount = currentBeatDuration / beatDuration;
        // Animations
        if (catfishAnimator.GetBool("Happy")) {
            currentTimeHappy -= dt;
            if (currentTimeHappy <= 0) {
                catfishAnimator.SetBool("Happy", false);
            }
        }
        if (catfishAnimator.GetBool("Angry")) {
            currentTimeAngry -= dt;
            if (currentTimeAngry <= 0) {
                catfishAnimator.SetBool("Angry", false);
            }
        }
    }

    public void ExplodeBubble()
    {
        Debug.Log("Bubble exploded");
        float fillAmount = currentBeatDuration / beatDuration;
        if(fillAmount > goodHitMargin)
        {
            Debug.Log("Good");
            StartCoroutine(ActivateAndDeactivate(goodIndicator));
            catfishAnimator.SetBool("Happy", true);
            catfishAnimator.SetBool("Angry", false);
            currentTimeHappy = timeHappy;
            // StartCoroutine(ActivateAndDeactivateAnimation("Happy"));
            currentPerformance++;
            currentPerformance = Mathf.Clamp(currentPerformance, -10, 10);
            performanceText.text = "Performance: " + currentPerformance;
            currentScore++;
            scoreText.text = "Score: +" + currentScore;
            // Bubble sounds
            bubbleEventEmitter.Play();
            bubbleEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
            // Music and performance
            if (currentPerformance > 5 && currentMusicIntensity == 0)
            {
                currentMusicIntensity = 1;
                musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            }
            if(currentPerformance >= 10 && currentMusicIntensity == 1)
            {
                currentMusicIntensity = 2;
                musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            }
        }
        else
        {
            Debug.Log("Bad");
            StartCoroutine(ActivateAndDeactivate(badIndicator));
            // StartCoroutine(ActivateAndDeactivateAnimation("Angry"));
            catfishAnimator.SetBool("Angry", true);
            catfishAnimator.SetBool("Happy", false);
            currentTimeAngry = timeAngry;
            currentPerformance--;
            currentPerformance = Mathf.Clamp(currentPerformance, -10, 10);
            performanceText.text = "Performance: " + currentPerformance;
            // Bubble sounds
            bubbleEventEmitter.Play();
            bubbleEventEmitter.EventInstance.setParameterByName("Burbuja", 0);
            // Music and performance
            if (currentPerformance <= 5 && currentMusicIntensity == 2)
            {
                currentMusicIntensity = 0;
                musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            }
            if (currentPerformance <= -10)
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

    // IEnumerator ActivateAndDeactivateAnimation(string state) {
    //     catfishAnimator.SetBool(state, true);
    //     yield return new WaitForSeconds(statesDurations[state]);
    //     catfishAnimator.SetBool(state, false);
    // }
}
