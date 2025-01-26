using UnityEngine;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    Animator bubbleAnimator;
    public Texture2D explodedBubbleTexture;
    public Sprite explodedBubbleSprite;

    private bool exploded = false;
    private Image buttonImage;
    private FMODUnity.StudioEventEmitter eventEmitter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbleAnimator = GetComponent<Animator>();
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
            bubbleAnimator.SetBool("Explosion", true);
            //eventEmitter.Play();
            //eventEmitter.EventInstance.setParameterByName("Burbuja", 1);
        }
        else
        {
            LevelManager.Instance.BadBubble();
            eventEmitter.Play();
            eventEmitter.EventInstance.setParameterByName("Burbuja", 0);
        }
    }
}
