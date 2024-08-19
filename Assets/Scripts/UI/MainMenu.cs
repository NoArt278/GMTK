using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;
    [SerializeField] private GameObject activeMenu;
    [SerializeField] private Slider bgmSlider, sfxSlider;
    private Vector3 inside, outside;

    private void Start()
    {
        inside = activeMenu.transform.localPosition;
        outside = inside - new Vector3(600f,0);
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

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
