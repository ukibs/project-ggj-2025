using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    
    [Header("Components")]
    //public GameObject catfish;
    public Animator catfishAnimator;
    public Animator bubbleAnimator;
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

    public JumperEffect[] jumpers;

    public ShakeEffect shakeEffect;

    public GameObject goodIndicator;
    public GameObject badIndicator;
    public TMP_Text scoreText;
    public TMP_Text performanceText;
    public TMP_Text fmodTimeText;
    public TMP_Text scoreTextCool;

    public Image rythmIndicatorImageL;
    public Image rythmIndicatorImageR;

    public Texture2D cursorTexture;

    int times;
    public TextMeshProUGUI streakText;
    double lastStreak;
    double streak;
    public Image streakImage;
    public Sprite[] streakStates;

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
    private int linesRolled = 0;
    private int tilesGenerated = 0;

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
        currentTimeAngry = timeAngry;
        currentTimeHappy = timeHappy;
        currentTimeWait = timeWait;

        musicEventEmitter = GameObject.Find("Music").GetComponent<FMODUnity.StudioEventEmitter>();
        bubbleEventEmitter = GameObject.Find("Bubble Effects").GetComponent<FMODUnity.StudioEventEmitter>();
        streakEventEmitter = GameObject.Find("Streak Effects").GetComponent<FMODUnity.StudioEventEmitter>();

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        //GenerateTiles();
        tileObject.SetActive(false);
        GenerateTile();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) {
            if (!catfishAnimator) {
                StartCoroutine(WaitAndGameOver());
            }
            return;
        }

        float dt = Time.deltaTime;
        int fmodTime;
        musicEventEmitter.EventInstance.getTimelinePosition(out fmodTime);
        fmodTimeText.text = "FTime: " + currentMusicIntensity + " - " + fmodTime;
        int partDuration = 16000;   // Lo que dura cada segmento
        //if (fmodPreviousTime < partDuration * currentMusicIntensity || 
        //    fmodPreviousTime > partDuration * (currentMusicIntensity + 1))
        //{            
        //    fmodPreviousTime = partDuration * (currentMusicIntensity);
        //}
        if (fmodPreviousTime > fmodTime ||
            fmodTime - fmodPreviousTime > partDuration)
        {
            fmodPreviousTime = partDuration * (currentMusicIntensity);
        }
        fmodDeltaTime = fmodTime - fmodPreviousTime;
        fmodPreviousTime = fmodTime;

        //Debug.Log("Fmod time: ")
        //currentBeatDuration += dt;
        currentBeatDuration += fmodDeltaTime;
        if (currentBeatDuration >= beatDuration)
        {
            currentBeatDuration -= beatDuration;
            // currentBeatDuration = 0;
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
                linesRolled++;
                if(linesRolled >= 6)
                {
                    Debug.Log(linesRolled + " lines rolled. Generating tile");
                    GenerateTile();
                    linesRolled = 0;
                    //tilesGenerated++;
                }
            }
        }
        // Barras de beat
        //beatFiller1.fillAmount = currentBeatDuration / beatDuration;
        //beatFiller2.fillAmount = currentBeatDuration / beatDuration;
        rythmIndicatorL.anchoredPosition = new Vector2(-((float)currentBeatDuration / (float)beatDuration * 150), 0);
        rythmIndicatorR.anchoredPosition = new Vector2(((float)currentBeatDuration / (float)beatDuration * 150), 0);
        // Animations
        // - Animacion de felicidad
        if (catfishAnimator) {
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
            // - Easter Egg
            if (UnityEngine.Random.Range(0f, 1f) < 0.001) {
                catfishAnimator.SetBool("EasterEgg", true);
            }
            if (catfishAnimator.GetBool("EasterEgg")) {
                AnimatorStateInfo stateInfo = catfishAnimator.GetCurrentAnimatorStateInfo(0);
                // Verifica si la animación ha terminado
                if (stateInfo.IsName("EasterEgg") && stateInfo.normalizedTime >= 1f && !catfishAnimator.IsInTransition(0)) {
                    catfishAnimator.SetBool("EasterEgg", false);
                }
            }
        }
        
        if (!hasClicked)
        {
            // Pierdes puntos
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameOver();
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

    void GenerateTile()
    {        
        RectTransform nextTile = Instantiate(tileObject, rollController.transform).GetComponent<RectTransform>();
        foreach (Transform line in nextTile) {
            foreach (Transform bubble in line) {
                if (UnityEngine.Random.Range(1,501) < 50) {
                    bubble.GetComponent<BubbleController>().exploded = true;
                }
            }
        }
        nextTile.gameObject.SetActive(true);

        nextTile.anchoredPosition = Vector2.up * 600 * (tilesGenerated + 1);
        tilesGenerated++;
    }

    void CheckPerformance()
    {
        //
        int currentStreakIntensity = Math.Min(currentPerformance / 5, maxMusicIntensity);
        shakeEffect.SetMultiplier(currentStreakIntensity);
        currentStreakIntensity = (int)MathF.Max(0, currentStreakIntensity);
        if(currentStreakIntensity != currentMusicIntensity)
        {
            Debug.Log("Current performance: " + currentPerformance + " - Current streak intensity: " + currentStreakIntensity + " - Current music intensity: " + currentMusicIntensity);
            if (currentStreakIntensity > currentMusicIntensity)
            {
                // Streak sounds
                streakEventEmitter.Play();
            }

            currentMusicIntensity = currentStreakIntensity;
            musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", currentMusicIntensity);
            currentHitMargin = goodHitMargin - (0.1f * currentMusicIntensity);
            beatMarginIndicatorL.anchoredPosition = new Vector2(-(currentHitMargin * 150), 0);
            beatMarginIndicatorR.anchoredPosition = new Vector2((currentHitMargin * 150), 0);
            //currentBeats = 0;
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
        //    beatMarginIndicatorL.anchoredPosition = new Vecursoerctor2(-(currentHitMargin * 150), 0);
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
            GameOver();
        }
    }

    public void GameOver()
    {
        // You lose
        // musicEventEmitter.Stop();
        // Pez muerto
        gameOver = true;
        musicEventEmitter.EventInstance.setParameterByName("MusicIntensity", 6);
        catfishAnimator.SetBool("Death", true);
        bubbleAnimator.SetBool("Destroy", true);
        catfishAnimator.AddComponent<PhyisicsEffect>();
        PlayerPrefs.SetInt("NewScore", currentScore);
    }

    IEnumerator WaitAndGameOver()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOver");
    }

    public bool ExplodeBubble()
    {
        if (gameOver) return false;
        //Debug.Log("Bubble exploded");
        // Reset wait time
        currentTimeWait = timeWait;
        if (catfishAnimator) {
            catfishAnimator.SetBool("Wait", false);
        }
        // 
        hasClicked = true;
        float fillAmount = (float)currentBeatDuration / (float)beatDuration;
        if(fillAmount <= currentHitMargin || 
            fillAmount > 0.90f) // Tolereancia extra
        {
            times += 1;
            //Debug.Log("Good");
            StartCoroutine(ActivateAndDeactivate(goodIndicator));
            if (catfishAnimator) {
                catfishAnimator.SetBool("Happy", true);
                catfishAnimator.SetBool("Angry", false);
            }
            currentTimeHappy = timeHappy;
            // StartCoroutine(ActivateAndDeactivateAnimation("Happy"));
            currentPerformance++;
            // currentPerformance = Mathf.Clamp(currentPerformance, -5, (maxMusicIntensity * 5) + 1);
            performanceText.text = "Performance: " + currentPerformance;
            currentScore += 1 * (int)MathF.Pow(2, currentMusicIntensity); // TODO: Con multiplicadores
            streakText.text = "x" + (int)MathF.Pow(2, currentMusicIntensity);
            scoreText.text = "Score: " + currentScore;
            //
            int decimals = (int)Math.Floor(Math.Log10(currentScore) + 1);
            string zeroes = "";
            for(int i = 0; i < 4 - decimals; i++)
            {
                zeroes += "0";
            }
            scoreTextCool.text = zeroes + currentScore;
            // Bubble sounds
            bubbleEventEmitter.Play();
            bubbleEventEmitter.EventInstance.setParameterByName("Burbuja", 1);
            //
            rythmIndicatorImageL.color = new Color(0, 1, 0);
            rythmIndicatorImageR.color = new Color(0, 1, 0);
            // 
            foreach (JumperEffect jumper in jumpers) {
                jumper.Play();
            }
            // Streak
            //streak = Math.Log10(times);
            //if (streak != lastStreak) {
            //    if (streak > 0) {
            //        streakText.text = "x" + streak;
            //    }
            //    streakText.GetComponent<JumperEffect>().Play();
            //}
            //lastStreak = streak;
            
            //if (streak < 5) {
            //    streakImage.sprite = streakStates[0];
            //} else if (streak >= 5 && streak < 12) {
            //    streakImage.sprite = streakStates[1];
            //} else if (streak >= 12 && streak < 20) {
            //    streakImage.sprite = streakStates[2];
            //} else if (streak >= 20) {
            //    streakImage.sprite = streakStates[3];
            //}
            streakImage.sprite = streakStates[currentMusicIntensity];
            return true;
        }
        else
        {

            BadPulsation();
            return false;
        }
    }

    public void BadBubble()
    {
        // Pierdes puntos
        BadPulsation();
    }

    public void BadPulsation()
    {
        times = 0;
        streak = 0;
        streakText.text = "";
        streakImage.sprite = streakStates[0];
        shakeEffect.distanceMultiplier = 0;

        //Debug.Log("Bad");
        StartCoroutine(ActivateAndDeactivate(badIndicator));
        // StartCoroutine(ActivateAndDeactivateAnimation("Angry"));
        if (catfishAnimator) {
            catfishAnimator.SetBool("Angry", true);
            catfishAnimator.SetBool("Happy", false);
        }
        currentTimeAngry = timeAngry;
        if (currentPerformance > 5)
            currentPerformance -= 5;
        else
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
}
