using UnityEngine;

public class TileController : MonoBehaviour
{
    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(rectTransform.anchoredPosition.y < -600)
        //Debug.Log("Tile y position: " + transform.position.y);
        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }
}
