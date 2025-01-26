using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{

    public GameObject shakeGameObject;
    public Animator catfishAnimator;
    bool waitDeath = false;
    bool exit = false;
    public bool menu = false;
    bool pressed;
    public bool changeScene;
    public CounterController counterController;
    Animator animator;
    BubbleMovement bubbleMovement;
    public InstructionsController instructionsController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        bubbleMovement = GetComponent<BubbleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (exit) {
            // Obtén el AnimatorStateInfo de la animación actual
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            // Verifica si la animación ha terminado
            if (stateInfo.IsName("Explosion")) {
                if (stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0)) {
                    Application.Quit();
                }
            }
            return;
        }
        if (!waitDeath) {
            if (pressed) {
                // Obtén el AnimatorStateInfo de la animación actual
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                // Verifica si la animación ha terminado
                if (stateInfo.IsName("Explosion")) {
                    if (stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0)) {
                        if (!catfishAnimator) {
                            if (!changeScene) {
                                Destroy(gameObject);
                            } else if (transform.childCount == 0) {
                                instructionsController.Play();
                            }
                        }
                    }
                }
                if (catfishAnimator) {
                    if (!menu) {
                        stateInfo = catfishAnimator.GetCurrentAnimatorStateInfo(0);
                        if (stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0)) {
                            catfishAnimator.AddComponent<PhyisicsEffect>();
                            waitDeath = true;
                        }
                    } else {
                        catfishAnimator.AddComponent<PhyisicsEffect>();
                        catfishAnimator.transform.SetParent(transform.parent.parent);
                        Destroy(gameObject);
                    }
                }
            }
        } else if (!catfishAnimator) {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void Play() {
        pressed = true;
        animator.SetBool("Destroy", true);
        if (counterController) {
            counterController.Add();
        }
        foreach (Transform child in transform) {
            if (!changeScene) {
                child.SetParent(transform.parent);
            }
            child.gameObject.SetActive(false);
            PhyisicsEffect phyisicsEffect = child.AddComponent<PhyisicsEffect>();
            if (bubbleMovement) {
                phyisicsEffect.velocity = bubbleMovement.direction * bubbleMovement.speed;
                phyisicsEffect.rotation = bubbleMovement.rotation;
            }
            child.gameObject.SetActive(true);
        }
        if (catfishAnimator) {
            catfishAnimator.SetBool("Death", true);
            // Silenciar sonido, etc.
        }
        if (shakeGameObject) {
            shakeGameObject.GetComponent<ShakeEffect>().Play();
            shakeGameObject.GetComponent<ShakeEffect>().IncreaseMultiplier();
        }
    }

    public void Exit() {
        exit = true;
        animator.SetBool("Destroy", true);
        foreach (Transform child in transform) {
            child.SetParent(transform.parent);
            child.gameObject.SetActive(false);
            PhyisicsEffect phyisicsEffect = child.AddComponent<PhyisicsEffect>();
            if (bubbleMovement) {
                phyisicsEffect.velocity = bubbleMovement.direction * bubbleMovement.speed;
                phyisicsEffect.rotation = bubbleMovement.rotation;
            }
            child.gameObject.SetActive(true);
        }
    }
}
