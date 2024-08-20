using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SidePanel : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu, clearMenu;
    [SerializeField] private Slider bgmSlider, sfxSlider;
    private GameManager gameManager;
    private Vector3 inside, outside;
    [SerializeField] private AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        outside = transform.localPosition;
        inside = outside + new Vector3(700f, 0);

        SetBGMVolume(PlayerPrefs.GetFloat("BGMVol", 100));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVol", 100));
    }

    private void OnEnable()
    {
        InputManager.playerInput.Player.Pause.performed += PauseGame;
    }

    private void OnDisable()
    {
        InputManager.playerInput.Player.Pause.performed -= PauseGame;
        transform.DOKill();
    }

    public void ShowClearUI()
    {
        clearMenu.SetActive(true);
        transform.DOLocalMove(inside,0.5f);
    }

    public void HideClearUI()
    {
        transform.DOLocalMove(outside, 0.5f).OnComplete(() =>
        {
            clearMenu.SetActive(false);
        });
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        if (pauseMenu.activeSelf)
        {
            HidePauseUI();
        } else
        {
            ShowPauseUI();
        }
    }

    public void ShowPauseUI()
    {
        if (pauseMenu.activeSelf || clearMenu.activeSelf) return;
        pauseMenu.SetActive(true);
        transform.DOLocalMove(inside, 0.5f).OnComplete(() =>
        {
            Time.timeScale = 0;
        });
    }

    public void HidePauseUI()
    {
        Time.timeScale = 1f;
        transform.DOLocalMove(outside, 0.5f).OnComplete(() =>
        {
            pauseMenu.SetActive(false);
        });
    }

    public void SetBGMVolume(float volume)
    {
        if (volume < 1)
        {
            volume = 0.001f;
        }
        mixer.SetFloat("BGMVol", Mathf.Log10(volume / 100) * 20f);
        bgmSlider.value = volume;
        PlayerPrefs.SetFloat("BGMVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume < 1)
        {
            volume = 0.001f;
        }
        mixer.SetFloat("SFXVol", Mathf.Log10(volume / 100) * 20f);
        sfxSlider.value = volume;
        PlayerPrefs.SetFloat("SFXVol", volume);
    }
}
