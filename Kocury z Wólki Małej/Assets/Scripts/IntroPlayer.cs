using StarterAssets;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroPlayer : MonoBehaviour
{
    public VideoPlayer introPlayer;
    private StarterAssetsInputs _input;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        introPlayer.loopPointReached += delegate(VideoPlayer vp) {SceneManager.LoadScene("Barn");};
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.shoot) {
            SceneManager.LoadScene("Barn");
            _input.shoot = false;
        }
    }
}
