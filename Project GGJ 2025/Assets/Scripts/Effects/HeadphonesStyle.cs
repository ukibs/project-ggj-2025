using UnityEngine;

public class HeadphonesStyle : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float dt = Time.deltaTime;
        float newScale = Mathf.Sin(Time.time * 15);
        transform.localScale = Vector3.one * (0.9f + (0.1f * newScale));
        Debug.Log(transform.localScale);
    }
}
