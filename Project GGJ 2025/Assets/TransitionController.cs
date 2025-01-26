using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{


    [SerializeField] bool closing;
    string goToScene;
    Image image;
    [SerializeField] float transitionTime;
    float currentTransitionTime;

    [SerializeField] GameObject[] toDeactivate;
    [SerializeField] GameObject[] toActivate;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = (!closing) ? 1 : 0; 

    }

    // Update is called once per frame
    void Update()
    {
        if (!closing) {
            if (currentTransitionTime < transitionTime) {
                currentTransitionTime += Time.deltaTime;
                image.fillAmount = Mathf.Lerp(1.1f, 0f, currentTransitionTime/transitionTime);
            } else {
                foreach (GameObject gameObject in toActivate) {
                    gameObject.SetActive(true);
                }
                image.raycastTarget = false;
            }
        } else if (!string.IsNullOrWhiteSpace(goToScene)) {

            if (currentTransitionTime < transitionTime) {
                currentTransitionTime += Time.deltaTime;
                image.fillAmount = Mathf.Lerp(0, 1.1f, currentTransitionTime/transitionTime);
            } else {
                SceneManager.LoadScene(goToScene);
            }
        }
    }


    public void GoTo(string scene) {
        foreach (GameObject gameObject in toDeactivate) {
            gameObject.SetActive(false);
        }
        goToScene = scene;
        Transform parent = transform.parent;
        transform.SetParent(null);
        transform.SetParent(parent);
    }
}
