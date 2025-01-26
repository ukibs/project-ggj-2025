using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyisicsEffect : MonoBehaviour
{
    public Vector3 velocity;
    public float rotation;
    float gravity = 400f;

    RectTransform rectTransform;
    Vector2 size;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        size = new Vector2(rectTransform.rect.width/2, rectTransform.rect.height/2);
        velocity.z = 0;
    }

    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;         
        transform.Rotate(rotation * new Vector3(0,0,1) * 20 * Time.deltaTime);
    
        Vector2[] points = {
            new Vector2(transform.position.x - size.x, transform.position.y - size.y),
            new Vector2(transform.position.x - size.x, transform.position.y + size.y),
            new Vector2(transform.position.x + size.x, transform.position.y - size.y),
            new Vector2(transform.position.x + size.x, transform.position.y + size.y)
        };
        bool outOfAxisX = true;
        bool outOfAxisY = true;
        foreach (Vector2 point in points) {
            outOfAxisX &= (point.x < 0 || point.x > Screen.width);
            outOfAxisY &= (point.y < 0 || point.y > Screen.height);
        }
        if (outOfAxisX || outOfAxisY) {
            Destroy(gameObject);
        }
    }

}
