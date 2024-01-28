using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject NextLevelScreen;
    [SerializeField] private GameObject Clown;
    [SerializeField] private GameObject Granny;
    [SerializeField] private TextMeshProUGUI BananaAmountText;
    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Button buttonTryAgain;
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonNextLevel;
    [SerializeField] private GameObject particleSmile;
    [SerializeField] private GameObject particleSad;
    [SerializeField] private AudioSource laughSound;
    [SerializeField] private AudioSource booSound;

    public int BananaAmount;
    public int Level;
    public int ClownAmounts;
    public int GrannyAmounts;

    private bool gameOver;
    private bool nextLevel;
    private bool pause;

    private void Awake()
    {
        Time.timeScale = 1;

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        buttonTryAgain.onClick.AddListener(TryAgain);
        buttonExit.onClick.AddListener(Exit);
        buttonNextLevel.onClick.AddListener(NextLevel);
    }

    private void Start()
    {
        Level = PlayerPrefs.GetInt("Level", 0);
        ClownAmounts = Level + 4;
        GrannyAmounts = Level + 3;
        BananaAmount = Level + 7;
        BananaAmountText.text = BananaAmount.ToString();
        LevelText.text = "LEVEL " + (Level + 1).ToString();
        InstantiateNPC();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowPauseScreen();
    }

    private void ShowPauseScreen()
    {
        pause = !pause;
        if (pause)
        {
            GameOverScreen.SetActive(true);
            GameOverText.text = "PAUSE";
            Time.timeScale = 0;
        }
        else
        {
            GameOverScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void InstantiateNPC()
    {
        for (int i = 0; i < ClownAmounts; i++)
        {
            Vector3 randomDestination = Random.insideUnitSphere * 5f;
            randomDestination = new Vector3(randomDestination.x, randomDestination.y, 0);
            Instantiate(Clown, randomDestination, Quaternion.identity);
        }

        for (int i = 0; i < GrannyAmounts; i++)
        {
            Vector3 randomDestination = Random.insideUnitSphere * 5f;
            randomDestination = new Vector3(randomDestination.x, randomDestination.y, 0);
            Instantiate(Granny, randomDestination, Quaternion.identity);
        }
    }

    public void GrannySlip()
    {
        booSound.Play();
        StartCoroutine(ShowParticle(particleSad));

        GrannyAmounts--;
        if (GrannyAmounts == 0 && !nextLevel)
        {
            gameOver = true;
            GameOverText.text = "GAME OVER!";
            StartCoroutine(OpenScreenWIthDealy(GameOverScreen));
        }
    }
    public void ClownSlip()
    {
        laughSound.Play();
        StartCoroutine(ShowParticle(particleSmile));

        ClownAmounts--;
        if (ClownAmounts == 0 && !gameOver)
        {
            nextLevel = true;
            StartCoroutine(OpenScreenWIthDealy(NextLevelScreen));
        }
    }
    IEnumerator OpenScreenWIthDealy(GameObject screen)
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        screen.SetActive(true);
    }

    IEnumerator ShowParticle(GameObject particle)
    {
        foreach (Transform child in particle.gameObject.transform)
        {
            child.GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(3);
        foreach (Transform child in particle.gameObject.transform)
        {
            child.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void BananaThrow()
    {
        BananaAmount--;
        BananaAmountText.text = BananaAmount.ToString();
    } 
    private void NextLevel()
    {
        PlayerPrefs.SetInt("Level", Level + 1);
        SceneManager.LoadScene(1);
    }
    private void Exit()
    {
        Application.Quit();
    }
    private void TryAgain()
    {
        SceneManager.LoadScene(1);
    }
}
