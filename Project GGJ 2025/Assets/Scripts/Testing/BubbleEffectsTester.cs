using UnityEngine;

public class BubbleEfectsTester : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter fmodComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fmodComponent = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fmodComponent.Play();
            fmodComponent.EventInstance.setParameterByName("Burbuja", 1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            fmodComponent.Play();
            fmodComponent.EventInstance.setParameterByName("Burbuja", 0);
        }
    }
}
