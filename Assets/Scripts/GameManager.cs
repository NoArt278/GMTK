using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static int maxLevel = 10;
    public static GameManager instance;
    private Coroutine checkIsPlaying;
    [SerializeField] private AudioSource bgm, sfx;
    [SerializeField] private AudioClip menuBGM, gameBGM;
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (instance == this)
            InputManager.playerInput.Enable();
    }

    private void OnDisable()
    {
        if (instance == this) 
            InputManager.playerInput.Disable();
    }

    public void CompleteLevel()
    {
        sfx.Play();
        ActivateCutoff(sfx);
    }

    public void ActivateCutoff(AudioSource source)
    {
        mixer.SetFloat("BGMCutoff", 1200f);
        if (checkIsPlaying != null)
        {
            StopCoroutine(checkIsPlaying);
        }
        checkIsPlaying = StartCoroutine(CheckStillPlaying(source));
    }

    IEnumerator CheckStillPlaying(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        mixer.SetFloat("BGMCutoff", 22000f);
        checkIsPlaying = null;
    }
}
