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
            GameObject newBubble = Instantiate(bubble);
            newBubble.transform.position = new Vector3(Random.Range(0, Screen.width), transform.position.y, 0);
            newBubble.transform.SetParent(canvas);
            newBubble.GetComponent<MoreBubbleMovement>().enabled = true;
            newBubble.GetComponent<PlayButtonController>().enabled = true;
            currentTimeSpawn = Random.Range(timeSpawnRange[0], timeSpawnRange[1]);
        } else {
            currentTimeSpawn -= Time.deltaTime;
        }
    }
}
