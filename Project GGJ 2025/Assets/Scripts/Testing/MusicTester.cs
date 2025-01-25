using UnityEngine;

public class MusicTester : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            fmodComponent.EventInstance.setParameterByName("MusicIntensity", 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fmodComponent.EventInstance.setParameterByName("MusicIntensity", 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fmodComponent.EventInstance.setParameterByName("MusicIntensity", 2);
        }
    }
}
