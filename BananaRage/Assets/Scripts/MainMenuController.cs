using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button rulesScreenButton;
    [SerializeField] private GameObject rulesScreen;

    private void Awake()
    {
        rulesScreenButton.onClick.AddListener(OnRulesClickAndStartGame);
        buttonStart.onClick.AddListener(OnStartClick);
    }

    private void OnStartClick()
    {
        rulesScreen.SetActive(true);
    }
    private void OnRulesClickAndStartGame()
    {
        SceneManager.LoadScene(1);
    }
}
