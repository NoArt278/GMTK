using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;
    [SerializeField] private GameObject activeMenu;
    [SerializeField] private Slider bgmSlider, sfxSlider;
    [SerializeField] private AudioMixer mixer;
    private Vector3 inside, outside;

    private void Start()
    {
        inside = activeMenu.transform.localPosition;
        outside = inside - new Vector3(600f,0);

        SetBGMVolume(PlayerPrefs.GetFloat("BGMVol", 100));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVol", 100));
    }

    public void ChangeMenu(int index)
    {
        activeMenu.transform.DOLocalMove(outside, 0.5f).OnComplete(() =>
        {
            activeMenu.SetActive(false);
            activeMenu.transform.localPosition += new Vector3(600f,0);
            menus[index].transform.localPosition -= new Vector3(600f,0);
            menus[index].SetActive(true);
            menus[index].transform.DOLocalMove(inside, 0.5f);
            activeMenu = menus[index];
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

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
