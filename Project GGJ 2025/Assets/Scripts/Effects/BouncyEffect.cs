using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyEffect : MonoBehaviour
{
    [SerializeField] float resetTime = 2f;
    [SerializeField] float maxDistance = 15f;
    [SerializeField] float maxDelay = 1f;
    [SerializeField] float totalTime = 2;
    [SerializeField] bool sense;
    List<Vector3> originalPositions;
    List<Vector3> goingToPositions;
    List<bool> comingBack;
    List<bool> done;
    List<float> delay;
    List<float> currentTimes;
    int dones = 0;

    void Start() {
        originalPositions = new();
        goingToPositions = new();
        currentTimes = new();
        delay = new();
        comingBack = new();
        done = new();
        foreach (Transform child in transform) {
            originalPositions.Add(child.position);
            goingToPositions.Add(child.position + Vector3.up*maxDistance);
            delay.Add(maxDelay);
            currentTimes.Add(0);
            comingBack.Add(false);
            done.Add(false);
        }
        delay[0] = 0;
    }

    void Update()
    {
        if (dones < transform.childCount) {
            if (!sense) {
                for (int i = 0; i < transform.childCount; i++) {
                    if (!done[i]) {
                        Transform child = transform.GetChild(i);
                        if (delay[i] > 0) {
                            if (i == 0 || delay[i-1] <= 0) {
                                delay[i] -= Time.deltaTime;
                            }
                        } else if (!comingBack[i]) {
                            if (currentTimes[i] < totalTime) {
                                currentTimes[i] += Time.deltaTime;
                                child.position = Vector3.Lerp(originalPositions[i], goingToPositions[i], currentTimes[i]/totalTime);
                            } else {
                                currentTimes[i] = 0;
                                child.position = goingToPositions[i];
                                comingBack[i] = true;
                            }
                        } else {
                            if (currentTimes[i] < totalTime) {
                                currentTimes[i] += Time.deltaTime;
                                child.position = Vector3.Lerp(goingToPositions[i], originalPositions[i], currentTimes[i]/totalTime);
                            } else {
                                currentTimes[i] = 0;
                                child.position = originalPositions[i];
                                done[i] = true;
                                dones += 1;
                            }
                        }
                    }
                }
            } else {
                for (int i = transform.childCount-1; i >= 0; i--) {
                    if (!done[i]) {
                        Transform child = transform.GetChild(i);
                        if (delay[i] > 0) {
                            if (i == transform.childCount-1 || delay[i+1] <= 0) {
                                delay[i] -= Time.deltaTime;
                            }
                        } else if (!comingBack[i]) {
                            if (currentTimes[i] < totalTime) {
                                currentTimes[i] += Time.deltaTime;
                                child.position = Vector3.Lerp(originalPositions[i], goingToPositions[i], currentTimes[i]/totalTime);
                            } else {
                                currentTimes[i] = 0;
                                child.position = goingToPositions[i];
                                comingBack[i] = true;
                            }
                        } else {
                            if (currentTimes[i] < totalTime) {
                                currentTimes[i] += Time.deltaTime;
                                child.position = Vector3.Lerp(goingToPositions[i], originalPositions[i], currentTimes[i]/totalTime);
                            } else {
                                currentTimes[i] = 0;
                                child.position = originalPositions[i];
                                done[i] = true;
                                dones += 1;
                            }
                        }
                    }
                }
            }

        } else {
            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset() {
        yield return new WaitForSeconds(resetTime);
        for (int i = 0; i < transform.childCount; i++) {
            delay[i] = maxDelay;
            comingBack[i] = false;
            done[i] = false;
        }
        dones = 0;
    }
}
