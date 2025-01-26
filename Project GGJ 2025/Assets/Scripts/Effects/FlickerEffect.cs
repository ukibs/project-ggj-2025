using System.Collections;
using UnityEngine;

public class FlickerEffect : MonoBehaviour
{
    public GameObject gameObjectToFlicker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FlickerEffectCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FlickerEffectCoroutine()
    {
        while (true)
        {
            gameObjectToFlicker.SetActive(!gameObjectToFlicker.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
