using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{

    public GameObject shakeGameObject;
    public Animator catfishAnimator;
    bool waitDeath = false;
    bool pressed;
    public bool changeScene;
    Animator animator;
    BubbleMovement bubbleMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        bubbleMovement = GetComponent<BubbleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
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
                            } else {
                                SceneManager.LoadScene("Game");
                            }
                        }
                    }
                }
                if (catfishAnimator) {
                    stateInfo = catfishAnimator.GetCurrentAnimatorStateInfo(0);
                    if (stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0)) {
                        catfishAnimator.AddComponent<PhyisicsEffect>();
                        waitDeath = true;
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
        if (catfishAnimator) {
            catfishAnimator.SetBool("Death", true);
            // Silenciar sonido, etc.
        }
        if (shakeGameObject) {
            shakeGameObject.GetComponent<ShakeEffect>().Play();
            shakeGameObject.GetComponent<ShakeEffect>().IncreaseMultiplier();
        }
    }
}
