using UnityEngine.UI;
using UnityEngine;

public class PaperSpriteController : MonoBehaviour
{


    [SerializeField] Sprite[] lights;
    [SerializeField] Sprite[] shadows;
    [SerializeField] Image lightsPanel;
    [SerializeField] Image shadowsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightsPanel.sprite = lights[Random.Range(0, lights.Length)];
        shadowsPanel.sprite = shadows[Random.Range(0, shadows.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
