using System;
using UnityEngine;
using UnityEngine.UI;

public class BubbleMovement : MonoBehaviour
{

    bool collision = false;
    Vector3 direction;
    float rotation;
    Image bubbleSprite;
    RectTransform bubbleRectTransform;
    Vector3 bounds;
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = UnityEngine.Random.insideUnitSphere * 5;
        direction.z = 0;
        rotation = UnityEngine.Random.Range(-1, 1);
        // 
        bubbleSprite = GetComponent<Image>();
        bubbleRectTransform = bubbleSprite.GetComponent<RectTransform>();
        // PIVOTE EN EL CENTRO!!!!!!!!!!
        bounds = new Vector3(bubbleRectTransform.rect.width/2, bubbleRectTransform.rect.height/2, 0);
        bounds.z = (float)Math.Sqrt(bounds.x * bounds.x + bounds.y * bounds.y);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{transform.position.y + bounds.y}, {transform.position.y - bounds.y}, {Screen.height}");
        bool outOfAxisX = (transform.position.x + bounds.x >= Screen.width || transform.position.x - bounds.x <= 0 || transform.position.y + bounds.x >= Screen.height);
        bool outOfAxisY = (transform.position.y + bounds.y >= Screen.height || transform.position.y - bounds.y <= 0  || transform.position.x + bounds.y >= Screen.width);
        if (!collision) {
            if (outOfAxisX) {
                direction.x *= -1;
                rotation *= -1;
                collision = true;
            }
            if (outOfAxisY) {
                direction.y *= -1;
                rotation *= -1;
                collision = true;
            }
        } else if (!outOfAxisX || !outOfAxisY) {
            collision = false;
        }

        transform.position = Vector3.Lerp(transform.position, transform.position + direction, speed * Time.deltaTime);
        transform.Rotate(rotation * new Vector3(0,0,1) * rotationSpeed * Time.deltaTime);
    }
}
