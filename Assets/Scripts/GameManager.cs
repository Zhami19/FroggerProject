using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //Get Scripts
    [SerializeField] FroggerPlayerScript _frogScript;
    Frogger_InputActions _actions;

    public GameObject playerPrefab;
    public Transform startPoint;

    //Screens
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject finalScoreScreen;

    //Scoring
    public ScoringZoneScript[] _scoringZoneScript;
    public int gameScore = 0;
    public int playerLives = 2;
    public int padCount;

    //Print Scores
    public TMP_Text scoreCount;
    public TMP_Text lifeCount;
    public TMP_Text finalScore;

    //Life Bar
    public float timeAmount = 30;
    public Image timeBar;

    Scene scene;

    private void Awake()
    {
        _actions = new Frogger_InputActions();
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        finalScoreScreen.SetActive(false);
    }

    private void Update()
    {
        if (_actions.Player.Start.triggered && scene.buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }

        scoreCount.text = gameScore.ToString();
        lifeCount.text = playerLives.ToString();
        finalScore.text = scoreCount.text;

        timeBar.fillAmount = timeAmount / 30;
        _frogScript = GameObject.FindGameObjectWithTag("Player").GetComponent<FroggerPlayerScript>();
        if (!_frogScript.isDead) timeAmount -= Time.deltaTime;
        if (timeAmount <= 0 && !_frogScript.isDead) _frogScript.TimeOut();
    }

    public void NewFrog()
    {
        foreach (ScoringZoneScript z in _scoringZoneScript)
            z.zoneTrigger = false;

        Instantiate(playerPrefab, startPoint.position, startPoint.rotation);
        timeAmount = 30;
    }

    public void WinScreen()
    {
        winScreen.SetActive(true);
        finalScoreScreen.SetActive(true);
        StartCoroutine(MainMenu());
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        finalScoreScreen.SetActive(true);
        StartCoroutine(MainMenu());
    }

    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
