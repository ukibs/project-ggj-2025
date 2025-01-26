using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MoreBubbleMovement : MonoBehaviour
{
    float sense;
    public float amplitude = 5f;  // Amplitud del movimiento (cuánto se moverá el objeto)
    public float frequency = 1f;  // Frecuencia del movimiento (qué tan rápido se moverá)
    private Vector3 startPosition;

    RectTransform rectTransform;
    Vector2 size;

    void Start()
    {
        // Guardamos la posición inicial del objeto
        startPosition = transform.position;
        sense = UnityEngine.Random.Range(1, 101) < 50 ? -1 : 1;
        rectTransform = GetComponent<RectTransform>();
        size = new Vector2(rectTransform.rect.width/2, rectTransform.rect.height/2);

    }

    void Update()
    {
        // Calculamos la nueva posición usando Mathf.Sin para crear un movimiento oscilante
        float sinValue = sense * Mathf.Sin(Time.time * frequency) * amplitude;

        // Aplicamos el movimiento solo en el eje deseado
        transform.position = new Vector3(startPosition.x + sinValue, transform.position.y + 1, 0);

        Vector2[] points = new Vector2[] {
            new Vector2(transform.position.x - size.x, transform.position.y - size.y),
            new Vector2(transform.position.x - size.x, transform.position.y + size.y),
            new Vector2(transform.position.x + size.x, transform.position.y - size.y),
            new Vector2(transform.position.x + size.x, transform.position.y + size.y),
        };
        bool outUpScreen = true;
        foreach (Vector2 point in points) {
            outUpScreen &= point.y > Screen.height;
        }
        if (outUpScreen) {
            Destroy(gameObject);
        }
    }
}