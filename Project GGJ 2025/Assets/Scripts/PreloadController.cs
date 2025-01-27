using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitAndLoad());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Menu");
    }
}
