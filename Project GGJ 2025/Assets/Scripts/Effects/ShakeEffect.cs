using UnityEngine;

public class ShakeEffect : MonoBehaviour
{

    Vector3 startPosition;
    Vector3 endPosition;

    public bool onStart = false;
    public bool loop = false;

    public float duration = 0f;
    float currentDuration;
    [Min(0.1f)]
    public float timeSpeed = 0.1f;        // Duración del movimiento
    public float distance = 4;
    public float distanceMultiplier = 1;
    float currentDistanceMultiplier;
    private float timeElapsed = 0f;    // Tiempo transcurrido

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        startPosition.z = 0;
        endPosition = startPosition + Random.insideUnitSphere * distance * distanceMultiplier;
        endPosition.z = 0;
        if (onStart || loop) {
            currentDuration = duration;
        }
        currentDistanceMultiplier = distanceMultiplier;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (timeSpeed <= 0) {
            timeSpeed = 0.1f;
        }
        float dt = Time.deltaTime;
        if (loop || currentDuration > 0) {
            // Llamar a la función EaseInOut para mover el objeto
            float t = timeElapsed / timeSpeed; // Normalizar el tiempo (de 0 a 1)
            // Usar una función de easing (por ejemplo, Ease-In-Out)
            float easedTime = FunctionsEffect.EaseInOut(t);
            // Interpolar la posición con el valor suavizado
            transform.position = Vector3.Lerp(startPosition, endPosition , easedTime);
            // Incrementar el tiempo
            timeElapsed += dt;
            // Detener el movimiento cuando haya llegado al destino
            if (timeElapsed >= timeSpeed) {
                timeElapsed = 0;
                endPosition = startPosition + Random.insideUnitSphere * distance * currentDistanceMultiplier;
                endPosition.z = 0;
            }
            if (currentDuration > 0) {
                currentDuration -= dt;
            } else if (loop) {
                currentDuration = duration;
            } else {
                currentDistanceMultiplier = distanceMultiplier;
            }
        }
    }

    public void Play() {
        currentDuration = duration;
    }

    public void SetMultiplier(int distanceMultiplierReceived) {
        distanceMultiplier = distanceMultiplierReceived;
        currentDistanceMultiplier = distanceMultiplier;
    }

    public void IncreaseMultiplier() {
        if (distanceMultiplier <= 4) {
            distanceMultiplier += 0.25f;
        } else {
            distanceMultiplier = 4;
        }
    }

    public void DecreaseMultiplier() {
        if (distanceMultiplier >= 1) {
            distanceMultiplier -= 0.25f;
        } else {
            distanceMultiplier = 1;
        }
    }
}
