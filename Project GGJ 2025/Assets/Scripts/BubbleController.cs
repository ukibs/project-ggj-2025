using UnityEngine;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    public Texture2D explodedBubbleTexture;
    public Sprite explodedBubbleSprite;

    private bool exploded = false;
    private Image buttonImage;
    private FMODUnity.StudioEventEmitter eventEmitter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonImage = GetComponent<Image>();
        eventEmitter = GameObject.Find("Bubble Effects").GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAndExplode()
    {
        if (!exploded)
        {
            exploded = true;
            buttonImage.sprite = explodedBubbleSprite;
            LevelManager.Instance.ExplodeBubble();
            //eventEmitter.Play();
            //eventEmitter.EventInstance.setParameterByName("Burbuja", 1);
        }
        else
        {
            eventEmitter.Play();
            eventEmitter.EventInstance.setParameterByName("Burbuja", 0);
        }
    }
}
