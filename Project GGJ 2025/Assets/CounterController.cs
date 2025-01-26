using NUnit.Framework;
using TMPro;
using UnityEngine;

public class CounterController : MonoBehaviour
{

    public bool hidden;

    public float transitionTime;
    float currentTransitionTime;
    int counter;
    bool hiding = true;
    public TextMeshProUGUI counterText;
    Vector3 startPosition;
    Vector3 endPosition;
    public float hideTime = 5f;
    bool go;
    float currentHideTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(transform.position.x, Screen.height + GetComponent<RectTransform>().rect.height/2, transform.position.z);
        startPosition = transform.position;
        endPosition = transform.position + new Vector3(0, -GetComponent<RectTransform>().rect.height, 0);
        currentHideTime = hideTime;
    }

    // Update is called once per frame
    void Update()
    {
        counterText.text = $"x{counter}";
        if (!hiding) {
            if (currentHideTime <= 0) {
                go = true;
                hiding = true;
            } else {
                currentHideTime -= Time.deltaTime;
            }
        }
        if (!go) {
            return;
        }

        if (hiding) {
            if (currentTransitionTime < transitionTime) {
                currentTransitionTime += Time.deltaTime;
                transform.position = Vector3.Lerp(endPosition, startPosition, currentTransitionTime/transitionTime);
            } else {
                transform.position = startPosition;
                currentTransitionTime = 0;
                go = false;
            }
        } else {
            if (currentTransitionTime < transitionTime) {
                currentTransitionTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, endPosition, currentTransitionTime/transitionTime);
            } else {
                transform.position = endPosition;
                currentTransitionTime = 0;
                go = false;
            }
        }
    }

    public void Add() {
        counter += 1;
        currentHideTime = hideTime;
        if (hiding) {
            if (currentTransitionTime != 0) {
                currentTransitionTime = transitionTime - currentTransitionTime;
            }
            hiding = false;
            go = true;
        }
    }
}
