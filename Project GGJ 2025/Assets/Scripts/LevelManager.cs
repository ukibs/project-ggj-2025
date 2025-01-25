using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    [Header("Components")]
    //public GameObject catfish;
    [SerializeField] float timeWait = 10f;
    float currentTimeWait;
    public Animator catfishAnimator;
    public RectTransform rollRt;
    public RollController rollController;
    //public Image beatFiller1;
    //public Image beatMarginFiller1;
    //public Image beatFiller2;
    //public Image beatMarginFiller2;
    public RectTransform beatMarginIndicatorL;
    public RectTransform beatMarginIndicatorR;
    public RectTransform rythmIndicatorL;
    public RectTransform rythmIndicatorR;

    public GameObject goodIndicator;
    public GameObject badIndicator;
    public TMP_Text scoreText;
    public TMP_Text performanceText;

    [Header("Parameters")]
    public float beatDuration = 1f;
    public int beatsToAdvance = 8;
    public float goodHitMargin = 0.75f;
    [SerializeField] float timeAngry = 5f;
    float currentTimeAngry;
    [SerializeField] float timeHappy = 5f;
    float currentTimeHappy;
    // Dictionary<string, float> statesDurations;
    float currentHitMargin;

    private static LevelManager instance;
    private FMODUnity.StudioEventEmitter musicEventEmitter;
    private FMODUnity.StudioEventEmitter bubbleEventEmitter;
    private FMODUnity.StudioEventEmitter streakEventEmitter;
    private float currentBeatDuration = 0;
    private int currentBeats = 0;
    private int currentPerformance = 0;
    private int currentScore = 0;
    private int currentMusicIntensity = 0;
    private bool hasClicked = false;

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
        currentHitMargin = goodHitMargin;
        //beatMarginFiller1.fillAmount = goodHitMargin;
        //beatMarginFiller2.fillAmount = goodHitMargin;
        beatMarginIndicatorL.anchoredPosition = new Vector2(-currentHitMargin * 150,0);
        beatMarginIndicatorR.anchoredPosition = new Vector2(currentHitMargin * 150, 0);
        //catfishAnimator = catfish.GetComponent<Animator>();
        // statesDurations = new() {
        //     {"Angry", timeAngry},
        //     {"Happy", timeHappy}
        // };
        currentTimeAngry = timeAngry;
        currentTimeHappy = timeHappy;
        currentTimeWait  = timeWait;

        musicEventEmitter = GameObject.Find("Music").GetComponent<FMODUnity.StudioEventEmitter>();
        bubbleEventEmitter = GameObject.Find("Bubble Effects").GetComponent<FMODUnity.StudioEventEmitter>();
        streakEventEmitter = GameObject.Find("Streak Effects").GetComponent<FMODUnity.StudioEventEmitter>();
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
            //musicEventEmitter.Stop();
            //musicEventEmitter.Play();
            //musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            currentBeats++;
            hasClicked = false;
            CheckPerformance();
            if (currentBeats >= beatsToAdvance)
            {
                currentBeats = 0;
                // rollRt.anchoredPosition -= new Vector2(0, 100);
                rollController.RollLine();
                // Debug.Log("Adavancing panel");
                musicEventEmitter.Stop();
                musicEventEmitter.Play();
                musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            }
        }
        // Barras de beat
        //beatFiller1.fillAmount = currentBeatDuration / beatDuration;
        //beatFiller2.fillAmount = currentBeatDuration / beatDuration;
        rythmIndicatorL.anchoredPosition = new Vector2(-150 +(currentBeatDuration / beatDuration * 150), 0);
        rythmIndicatorR.anchoredPosition = new Vector2(150 -(currentBeatDuration / beatDuration * 150), 0);
        // Animations
        // - Animacion de felicidad
        if (catfishAnimator.GetBool("Happy")) {
            currentTimeHappy -= dt;
            if (currentTimeHappy <= 0) {
                catfishAnimator.SetBool("Happy", false);
            }
        }
        // - Animacion de enfado
        if (catfishAnimator.GetBool("Angry")) {
            currentTimeAngry -= dt;
            if (currentTimeAngry <= 0) {
                catfishAnimator.SetBool("Angry", false);
            }
        }
        // - Animacion de espera
        if (!catfishAnimator.GetBool("Angry") && !catfishAnimator.GetBool("Happy")) {
            if (currentTimeWait >= 0) {
                currentTimeWait -= dt;
            } else {
                catfishAnimator.SetBool("Wait", true);
            }
        }
        //
        //CheckPerformance();
    }

    void CheckPerformance()
    {
        // Music and performance
        if (currentPerformance > 5 && currentMusicIntensity == 0)
        {
            currentMusicIntensity = 1;
            musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            currentHitMargin = goodHitMargin - 0.1f;
            beatMarginIndicatorL.anchoredPosition = new Vector2(-150+(currentHitMargin * 150), 0);
            beatMarginIndicatorR.anchoredPosition = new Vector2(150-(currentHitMargin * 150), 0);
            currentBeats = 0;
            // Streak sounds
            streakEventEmitter.Play();
            //streakEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
        }
        if (currentPerformance >= 10 && currentMusicIntensity == 1)
        {
            currentMusicIntensity = 2;
            musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            currentHitMargin = goodHitMargin - 0.2f;
            beatMarginIndicatorL.anchoredPosition = new Vector2(-150 + (currentHitMargin * 150), 0);
            beatMarginIndicatorR.anchoredPosition = new Vector2(150 - (currentHitMargin * 150), 0);
            currentBeats = 0;
            // Streak sounds
            streakEventEmitter.Play();
            //streakEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
        }
        // Music and performance
        if (currentPerformance <= 5 && currentMusicIntensity == 2)
        {
            currentMusicIntensity = 0;
            musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            currentHitMargin = goodHitMargin + 0.0f;
            beatMarginIndicatorL.anchoredPosition = new Vector2(-150 + (currentHitMargin * 150), 0);
            beatMarginIndicatorR.anchoredPosition = new Vector2(150 - (currentHitMargin * 150), 0);
            currentBeats = 0;
        }
        if (currentPerformance <= -10)
        {
            // You lose
        }
    }

    public void ExplodeBubble()
    {
        Debug.Log("Bubble exploded");
        // Reset wait time
        currentTimeWait  = timeWait;
        catfishAnimator.SetBool("Wait", false);
        // 
        hasClicked = true;
        float fillAmount = currentBeatDuration / beatDuration;
        if(fillAmount <= goodHitMargin)
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
            currentScore += 1 + currentMusicIntensity;
            scoreText.text = "Score: +" + currentScore;
            // Bubble sounds
            bubbleEventEmitter.Play();
            bubbleEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
            
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
