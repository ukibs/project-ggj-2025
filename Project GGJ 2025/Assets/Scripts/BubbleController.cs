using UnityEngine;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    public Texture2D explodedBubbleTexture;
    public Sprite explodedBubbleSprite;

    private bool exploded = false;
    private Image buttonImage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonImage = GetComponent<Image>();
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
        }
    }
}
