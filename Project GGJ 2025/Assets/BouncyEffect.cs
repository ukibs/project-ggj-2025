using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BouncyEffect : MonoBehaviour
{

    [SerializeField] float resetTime = 2f;
    [SerializeField] float maxDistance = 15f;
    [SerializeField] float maxDelay = 1f;
    [SerializeField] float totalTime = 2;
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
            StartCoroutine(Reset());
        }
    }



//     // Update is called once per frame
//     void Update()
//     {
//     // Asegúrate de que el texto se haya actualizado.
//     textMesh.ForceMeshUpdate();
//     TMP_TextInfo textInfo = textMesh.textInfo;

//     // Itera sobre todos los caracteres del texto.
//     for (int i = 0; i < textInfo.characterCount; ++i)
//     {
//         TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];
//         // Si el carácter está visible.
//         if (!characterInfo.isVisible) {
//             continue;
//         }

//         Vector3[] vertices = textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;
 
//         // Modificar los vértices de los 4 puntos del carácter.
//         for (int j = 0; j < 4; ++j) 
//         {
//             Vector3 origin = vertices[characterInfo.vertexIndex + j];
//             vertices[characterInfo.vertexIndex + j] = origin + new Vector3(0, Time.time*2f /*Mathf.Sin(Time.time*2f + origin.x*0.01f) * 10f*/, 0); 
//         }
//     }

//     // Actualizar la malla con los nuevos vértices.
//     for (int i = 0; i < textInfo.meshInfo.Length; ++i)
//     {
//         TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
//         meshInfo.mesh.vertices = meshInfo.vertices;
//         textMesh.UpdateGeometry(meshInfo.mesh, i);
//     }
// }

    IEnumerator Reset() {
        print("Hello?");
        yield return new WaitForSeconds(resetTime);
        for (int i = 0; i < transform.childCount; i++) {
            delay[i] = maxDelay;
            comingBack[i] = false;
            done[i] = false;
        }
        dones = 0;
    }
}
