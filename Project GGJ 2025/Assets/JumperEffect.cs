using UnityEngine;


public class JumperEffect : MonoBehaviour
{

    Vector3 startPosition;      // Posición inicial
    Vector3 endPosition_1 = new Vector3(1.5f, 1.5f, 1.5f);        // Posición final
    Vector3 endPosition_2 = new Vector3(1.3f, 1.3f, 1.3f);
    Vector3 endPosition_3 = new Vector3(1.4f, 1.4f, 1.4f);
    public float duration = 0.25f;        // Duración del movimiento
    private float timeElapsed = 0f;    // Tiempo transcurrido

    int state = 0;


    RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Hola?");
            state = 0;
        }

        if (state == 0) {
            // Llamar a la función EaseInOut para mover el objeto
            float t = timeElapsed / duration; // Normalizar el tiempo (de 0 a 1)
            // Usar una función de easing (por ejemplo, Ease-In-Out)
            float easedTime = EaseInOut(t);
            // Interpolar la posición con el valor suavizado
            transform.localScale = Vector3.Lerp(startPosition, endPosition_1, easedTime);
            // Incrementar el tiempo
            timeElapsed += Time.deltaTime;
            // Detener el movimiento cuando haya llegado al destino
            if (timeElapsed >= duration) {
                timeElapsed = 0;
                state = 1;
            }
        } else if (state == 1) {
            float t = timeElapsed / 0.05f;
            float easedTime = EaseIn(t);
            transform.localScale = Vector3.Lerp(endPosition_1, endPosition_2, easedTime);
            // Incrementar el tiempo
            timeElapsed += Time.deltaTime;
            // Detener el movimiento cuando haya llegado al destino
            if (timeElapsed >= duration) {
                timeElapsed = 0;
                state = 2;
            }
        } else if (state == 2) {
            float t = timeElapsed / 0.05f;
            float easedTime = EaseOut(t);
            transform.localScale = Vector3.Lerp(endPosition_2, endPosition_3, easedTime);
            // Incrementar el tiempo
            timeElapsed += Time.deltaTime;
            // Detener el movimiento cuando haya llegado al destino
            if (timeElapsed >= duration) {
                timeElapsed = 0;
                state = 3;
            }
        } else if (state == 3) {
            float t = timeElapsed / duration;
            float easedTime = EaseInOut(t);
            transform.localScale = Vector3.Lerp(endPosition_3, startPosition, easedTime);
            // Incrementar el tiempo
            timeElapsed += Time.deltaTime;
            // Detener el movimiento cuando haya llegado al destino
            if (timeElapsed >= duration) {
                timeElapsed = 0;
                state = 4;
            }
        }

    }

    float EaseIn(float t)
    {
        return t * t;  // t^2
    }

    float EaseOut(float t)
    {
        return 1 - (1 - t) * (1 - t);  // 1 - (1 - t)^2
    }

    float EaseInOut(float t)
    {
        return 0.5f * (1 - Mathf.Cos(Mathf.PI * t));  // 0.5 * (1 - cos(π * t))
    }
}
