using System.Collections;
using UnityEngine;

public class RollController : MonoBehaviour
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
        
    }

    public void RollLine()
    {
        StartCoroutine(RollLineCoroutine());
    }

    IEnumerator RollLineCoroutine()
    {
        for(int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.05f);
            rectTransform.anchoredPosition -= new Vector2(0, 5);
        }
    }
}
