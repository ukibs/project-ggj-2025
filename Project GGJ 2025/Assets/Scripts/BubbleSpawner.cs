using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public Transform canvas;
    public GameObject bubble;
    public float timeSpawn = 1f;
    float[] timeSpawnRange;
    float currentTimeSpawn;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeSpawnRange = new float[]{timeSpawn*1/4, timeSpawn*3/4};
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimeSpawn <= 0) {
            GameObject newBubbleParent = Instantiate(bubble);
            GameObject newCatfish = newBubbleParent.transform.GetChild(0).gameObject;
            GameObject newBubble = newBubbleParent.transform.GetChild(1).gameObject;

            newBubbleParent.transform.position = new Vector3(Random.Range(0, Screen.width), transform.position.y, 0);
            newBubbleParent.transform.SetParent(canvas);
            newBubbleParent.GetComponent<MoreBubbleMovement>().enabled = true;
            PlayButtonController playButtonController = newBubble.GetComponent<PlayButtonController>();
            playButtonController.catfishAnimator = newCatfish.GetComponent<Animator>();
            playButtonController.enabled = true;
            currentTimeSpawn = Random.Range(timeSpawnRange[0], timeSpawnRange[1]);
        } else {
            currentTimeSpawn -= Time.deltaTime;
        }
    }
}
