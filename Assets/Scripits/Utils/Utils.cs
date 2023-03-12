using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class Utils : MonoBehaviour
{
    public GameObject pauseButtons;

    public RuntimeAnimatorController animatorController;
    public GameObject map;
    public Sprite warrirorBasic;
    public Sprite selection;
    public Sprite HP;
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void SwapPauseMenuGroaps(GameObject selection)
    {
        SwapActive(selection);
        SwapActive(pauseButtons);
    }
    public void SwapActive(GameObject gameObjct)
    {
        gameObjct.SetActive(!gameObjct.activeSelf);
    }
    public void SpawnHuman()
    {
        SpawningHandler.SpawnEntity(warrirorBasic, animatorController, selection, HP);
    }
    public void SwapMap()
    {
        map.SetActive(!map.activeSelf);
    }
}
