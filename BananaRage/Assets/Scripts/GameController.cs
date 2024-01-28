using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;

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
    [SerializeField] private TextMeshProUGUI ClownAmountText;
    [SerializeField] private Button buttonTryAgain;
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonNextLevel;
    [SerializeField] private GameObject particleSmile;
    [SerializeField] private GameObject particleSad;
    [SerializeField] private GameObject bananaFrenzyHolder;
    [SerializeField] private AudioSource laughSound;
    [SerializeField] private AudioSource booSound;

    public int BananaAmount;
    public int Level;
    public int ClownAmounts;
    public int GrannyAmounts;

    private bool gameOver;
    private bool nextLevel;
    private bool pause;
    private int clownsInRow;
    private int bananaFrenzyTreshlod = 4;

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
        clownsInRow = 0;
        Level = PlayerPrefs.GetInt("Level", 0);
        ClownAmounts = Level + 4;
        GrannyAmounts = Level + 3;
        BananaAmount = Level + 7;
        BananaAmountText.text = BananaAmount.ToString();
        ClownAmountText.text = ClownAmounts.ToString();
        LevelText.text = "LEVEL " + (Level + 1).ToString();
        InstantiateNPC();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowPauseScreen();

        if (Input.GetKeyDown(KeyCode.Tab) && (pause || gameOver)) buttonExit.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.Space) && (pause || gameOver)) buttonTryAgain.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.Space) && nextLevel) buttonNextLevel.onClick.Invoke();

        if (Time.timeScale == 0) Cursor.visible = true;
        else Cursor.visible = false;
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
    private IEnumerator CheckBananasAmount()
    {
        yield return new WaitForEndOfFrame();

        List<BananaPeel> bananas = FindObjectsOfType<BananaPeel>().ToList();

        if (bananas.Count == 0 && BananaAmount == 0 && !gameOver && !nextLevel)
        {
            gameOver = true;
            StartCoroutine(OpenScreenWIthDealy(GameOverScreen));
        }
        else if (bananas.Count == 0 && BananaAmount < ClownAmounts && !gameOver && !nextLevel)
        {
            gameOver = true;
            StartCoroutine(OpenScreenWIthDealy(GameOverScreen));
        }


    }
    public void GrannySlip()
    {
        StartCoroutine(CheckBananasAmount());
        booSound.Play();
        StartCoroutine(ShowParticle(particleSad));

        clownsInRow = 0;

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
        StartCoroutine(CheckBananasAmount());

        laughSound.Play();
        StartCoroutine(ShowParticle(particleSmile));

        clownsInRow++;

        ClownAmounts--;
        ClownAmountText.text = ClownAmounts.ToString();
        if (ClownAmounts == 0 && !gameOver)
        {
            nextLevel = true;
            StartCoroutine(OpenScreenWIthDealy(NextLevelScreen));
        }
        else if (clownsInRow >= bananaFrenzyTreshlod) BananaFrenzy();
    }

    private void BananaFrenzy()
    {
        BananaAmount += 4;
        BananaAmountText.text = BananaAmount.ToString();
        clownsInRow = 0;
        bananaFrenzyHolder.SetActive(true);
        FrenzyAnimation();
    }

    private void FrenzyAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(bananaFrenzyHolder.transform.DOScale(Vector3.zero, 1).From()).SetEase(Ease.OutBack).AppendInterval(2).OnComplete(() => bananaFrenzyHolder.SetActive(false));
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
