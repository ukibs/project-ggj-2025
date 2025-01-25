using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{

    [SerializeField] ParticleSystem particleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play() {
        if (particleSystem) {
            particleSystem.Play();
        }
        SceneManager.LoadScene("Game");
    }
}
