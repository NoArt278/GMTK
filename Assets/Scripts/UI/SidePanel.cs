using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidePanel : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu, clearMenu;
    [SerializeField] private Slider bgmSlider, sfxSlider;
    private GameManager gameManager;
    private Vector3 inside, outside;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        outside = transform.localPosition;
        inside = outside + new Vector3(700f, 0);
        bgmSlider.value = gameManager.bgmVolume;
        sfxSlider.value = gameManager.sfxVolume;
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

    public void ShowPauseUI()
    {
        if (pauseMenu.activeSelf) return;
        pauseMenu.SetActive(true);
        transform.DOLocalMove(inside, 0.5f);
    }

    public void HidePauseUI()
    {
        transform.DOLocalMove(outside, 0.5f).OnComplete(() =>
        {
            pauseMenu.SetActive(false);
        });
    }

    public void UpdateVolume()
    {
        gameManager.bgmVolume = bgmSlider.value;
        gameManager.sfxVolume = sfxSlider.value;
    }
}
