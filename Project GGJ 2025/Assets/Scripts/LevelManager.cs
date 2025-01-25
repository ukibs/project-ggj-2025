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
    public TMP_Text fmodTimeText;

    public Image rythmIndicatorImageL;
    public Image rythmIndicatorImageR;

    public GameObject tileObject;

    [Header("Parameters")]
    public int beatDuration = 500;
    public int beatsToAdvance = 8;
    public float goodHitMargin = 0.75f;
    public int maxMusicIntensity = 3;
    [SerializeField] float timeAngry = 5f;
    float currentTimeAngry;
    [SerializeField] float timeHappy = 5f;
    float currentTimeHappy;
    [SerializeField] float timeWait = 10f;
    float currentTimeWait;
    // Dictionary<string, float> statesDurations;
    float currentHitMargin;

    private static LevelManager instance;
    private FMODUnity.StudioEventEmitter musicEventEmitter;
    private FMODUnity.StudioEventEmitter bubbleEventEmitter;
    private FMODUnity.StudioEventEmitter streakEventEmitter;
    private int currentBeatDuration = 0;
    private int currentBeats = 0;
    private int currentPerformance = 0;
    private int currentScore = 0;
    private int currentMusicIntensity = 0;
    private bool hasClicked = false;
    private int fmodPreviousTime = 0;
    private int fmodDeltaTime = 0;
    private bool gameOver = false;

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
        currentTimeWait = timeWait;

        musicEventEmitter = GameObject.Find("Music").GetComponent<FMODUnity.StudioEventEmitter>();
        bubbleEventEmitter = GameObject.Find("Bubble Effects").GetComponent<FMODUnity.StudioEventEmitter>();
        streakEventEmitter = GameObject.Find("Streak Effects").GetComponent<FMODUnity.StudioEventEmitter>();

        GenerateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        float dt = Time.deltaTime;
        int fmodTime;
        musicEventEmitter.EventInstance.getTimelinePosition(out fmodTime);
        fmodTimeText.text = "FTime: " + fmodTime;
        if (fmodPreviousTime > fmodTime)
        {
            fmodPreviousTime = 8000 * (currentMusicIntensity);
        }
        fmodDeltaTime = fmodTime - fmodPreviousTime;
        fmodPreviousTime = fmodTime;

        //Debug.Log("Fmod time: ")
        //currentBeatDuration += dt;
        currentBeatDuration += fmodDeltaTime;
        if (currentBeatDuration >= beatDuration)
        {
            //currentBeatDuration -= beatDuration;
            currentBeatDuration = 0;
            //
            rythmIndicatorImageL.color = new Color(1, 1, 1);
            rythmIndicatorImageR.color = new Color(1, 1, 1);
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
                //musicEventEmitter.Stop();
                //musicEventEmitter.Play();
                //musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
                //fmodPreviousTime = 16000 * (currentMusicIntensity);
                //Debug.Log("Current music inteisity: " + currentMusicIntensity);
            }
        }
        // Barras de beat
        //beatFiller1.fillAmount = currentBeatDuration / beatDuration;
        //beatFiller2.fillAmount = currentBeatDuration / beatDuration;
        rythmIndicatorL.anchoredPosition = new Vector2(-((float)currentBeatDuration / (float)beatDuration * 150), 0);
        rythmIndicatorR.anchoredPosition = new Vector2(((float)currentBeatDuration / (float)beatDuration * 150), 0);
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
        if (!catfishAnimator.GetBool("Angry") && !catfishAnimator.GetBool("Happy"))
        {
            if (currentTimeWait >= 0)
            {
                currentTimeWait -= dt;
            }
            else
            {
                catfishAnimator.SetBool("Wait", true);
            }
        }
        //
        if (!hasClicked)
        {
            // Pierdes puntos
        }
    }

    void GenerateTiles()
    {
        for(int i = 0; i < 10; i++)
        {
            RectTransform nextTile = Instantiate(tileObject, rollController.transform).GetComponent<RectTransform>();
            nextTile.anchoredPosition = Vector2.up * 600 * (i + 1);
        }
    }

    void CheckPerformance()
    {
        //
        int currentStreakIntensity = currentPerformance / 5;
        currentStreakIntensity = (int)MathF.Max(0, currentStreakIntensity);
        if(currentStreakIntensity != currentMusicIntensity)
        {
            Debug.Log("Current performance: " + currentPerformance + " - Current streak intensity: " + currentStreakIntensity + " - Current music intensity: " + currentMusicIntensity);
            currentMusicIntensity = currentStreakIntensity;
            musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            currentHitMargin = goodHitMargin - (0.1f * currentMusicIntensity);
            beatMarginIndicatorL.anchoredPosition = new Vector2(-(currentHitMargin * 150), 0);
            beatMarginIndicatorR.anchoredPosition = new Vector2((currentHitMargin * 150), 0);
            //currentBeats = 0;
            // Streak sounds
            streakEventEmitter.Play();
            //streakEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
        }

        // Music and performance
        //if (currentPerformance > 5 && currentMusicIntensity == 0)
        //{
        //    currentMusicIntensity = 1;
        //    musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
        //    currentHitMargin = goodHitMargin - 0.1f;
        //    beatMarginIndicatorL.anchoredPosition = new Vector2(-(currentHitMargin * 150), 0);
        //    beatMarginIndicatorR.anchoredPosition = new Vector2((currentHitMargin * 150), 0);
        //    currentBeats = 0;
        //    // Streak sounds
        //    streakEventEmitter.Play();
        //    //streakEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
        //}
        //if (currentPerformance >= 10 && currentMusicIntensity == 1)
        //{
        //    currentMusicIntensity = 2;
        //    musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
        //    currentHitMargin = goodHitMargin - 0.2f;
        //    beatMarginIndicatorL.anchoredPosition = new Vector2(-(currentHitMargin * 150), 0);
        //    beatMarginIndicatorR.anchoredPosition = new Vector2((currentHitMargin * 150), 0);
        //    currentBeats = 0;
        //    // Streak sounds
        //    streakEventEmitter.Play();
        //    //streakEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
        //}
        //// Music and performance
        //if (currentPerformance <= 5 && currentMusicIntensity == 2)
        //{
        //    currentMusicIntensity = 0;
        //    musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
        //    currentHitMargin = goodHitMargin + 0.0f;
        //    beatMarginIndicatorL.anchoredPosition = new Vector2(-(currentHitMargin * 150), 0);
        //    beatMarginIndicatorR.anchoredPosition = new Vector2((currentHitMargin * 150), 0);
        //    currentBeats = 0;
        //}
        if (currentPerformance <= -5)
        {
            // You lose
            musicEventEmitter.Stop();
            // Pez muerto
            gameOver = true;
            musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", 6);
        }
    }

    public void ExplodeBubble()
    {
        if (gameOver) return;
        //Debug.Log("Bubble exploded");
        // Reset wait time
        currentTimeWait = timeWait;
        catfishAnimator.SetBool("Wait", false);
        // 
        hasClicked = true;
        float fillAmount = (float)currentBeatDuration / (float)beatDuration;
        if(fillAmount <= currentHitMargin)
        {
            //Debug.Log("Good");
            StartCoroutine(ActivateAndDeactivate(goodIndicator));
            catfishAnimator.SetBool("Happy", true);
            catfishAnimator.SetBool("Angry", false);
            currentTimeHappy = timeHappy;
            // StartCoroutine(ActivateAndDeactivateAnimation("Happy"));
            currentPerformance++;
            currentPerformance = Mathf.Clamp(currentPerformance, -5, (maxMusicIntensity * 5) + 1);
            performanceText.text = "Performance: " + currentPerformance;
            currentScore += 1 + currentMusicIntensity;
            scoreText.text = "Score: " + currentScore;
            // Bubble sounds
            bubbleEventEmitter.Play();
            bubbleEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
            //
            rythmIndicatorImageL.color = new Color(0, 1, 0);
            rythmIndicatorImageR.color = new Color(0, 1, 0);
        }
        else
        {
            BadPulsation();
        }
    }

    public void BadBubble()
    {
        // Pierdes puntos
        BadPulsation();
    }

    public void BadPulsation()
    {
        //Debug.Log("Bad");
        StartCoroutine(ActivateAndDeactivate(badIndicator));
        // StartCoroutine(ActivateAndDeactivateAnimation("Angry"));
        catfishAnimator.SetBool("Angry", true);
        catfishAnimator.SetBool("Happy", false);
        currentTimeAngry = timeAngry;
        currentPerformance--;
        currentPerformance = Mathf.Clamp(currentPerformance, -5, (maxMusicIntensity * 5) + 1);
        performanceText.text = "Performance: " + currentPerformance;
        // Bubble sounds
        bubbleEventEmitter.Play();
        bubbleEventEmitter.EventInstance.setParameterByName("Burbuja", 0);
        //
        rythmIndicatorImageL.color = new Color(1, 0, 0);
        rythmIndicatorImageR.color = new Color(1, 0, 0);
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
