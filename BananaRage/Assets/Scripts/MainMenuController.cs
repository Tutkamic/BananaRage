using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button buttonStart;

    private void Awake()
    {
        buttonStart.onClick.AddListener(OnStartClick);
    }

    private void OnStartClick()
    {
        SceneManager.LoadScene(1);
    }
}
