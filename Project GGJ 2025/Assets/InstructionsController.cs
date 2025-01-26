using UnityEngine;

public class InstructionsController : MonoBehaviour
{

    public GameObject menu;
    Vector3 menuPosition;
    public GameObject instructions;
    Vector3 instructionsPosition;
    bool go = false;
    public float transitionTime = 1f;
    float currentTransitionTime;
    public float readTime = 5f;
    public TransitionController transitionController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuPosition = menu.transform.position;
        instructionsPosition = instructions.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if (!go) {
            return;
        } else if (currentTransitionTime < transitionTime) {
            currentTransitionTime += dt;
            float progress = currentTransitionTime/transitionTime;
            menu.transform.position = Vector3.Lerp(menuPosition, new Vector3(-Screen.width/2, menuPosition.y, 0), progress);
            instructions.transform.position = Vector3.Lerp(instructionsPosition, new Vector3(Screen.width/2, menuPosition.y, 0), progress);
        } else if (readTime > 0) {
            readTime -= dt;
        } else {
            transitionController.GoTo("Game");
        }
    }

    public void Play() {
        go = true;
    }
}
