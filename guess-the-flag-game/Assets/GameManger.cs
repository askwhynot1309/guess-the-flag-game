using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AstraInputController inputController;

    [Header("UI")]
    public RawImage flagImage;
    public Button answerButton1;
    public Button answerButton2;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject timeUpPanel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    public TextMeshProUGUI highscoreText;

    private List<Country> countries;
    private Country correctCountry;
    private float timeRemaining = 6f;
    private int score = 0;
    private bool isGameActive = true;

    private FootDetector detector1;
    private FootDetector detector2;


    void Start()
    {
        SoundManager.Instance.PlayMusic();
        countries = CountryLoader.LoadCountries();
        timeUpPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        ShowNewFlag();
        StartCoroutine(GameAPI.Instance.GetHighScore(
        score =>
        {
            Debug.Log("Fetched high score: " + score);
            highscoreText.text = "Highscore: " + score.ToString();
        },
        error =>
        {
            Debug.LogError("Failed to fetch high score: " + error);
        }));

        if (inputController == null)
        {
            inputController = FindFirstObjectByType<AstraInputController>();
        }

        if (inputController != null)
        {
            inputController.OnClickEvent.AddListener(HandleFootClick);
        }

        detector1 = answerButton1.GetComponent<FootDetector>();
        detector2 = answerButton2.GetComponent<FootDetector>();
    }

    void HandleFootClick()
    {
        if (!isGameActive) return;

        if (detector1 != null && detector1.IsFootOver && answerButton1.interactable)
        {
            answerButton1.onClick.Invoke();
        }
        else if (detector2 != null && detector2.IsFootOver && answerButton2.interactable)
        {
            answerButton2.onClick.Invoke();
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            EndGame();
        }
    }

    void ShowNewFlag()
    {
        int correctIndex = Random.Range(0, countries.Count);
        correctCountry = countries[correctIndex];

        Country wrongCountry;
        do
        {
            wrongCountry = countries[Random.Range(0, countries.Count)];
        } while (wrongCountry.name == correctCountry.name);

        flagImage.texture = correctCountry.texture;

        bool showCorrectFirst = Random.Range(0, 2) == 0;

        if (showCorrectFirst)
        {
            SetButton(answerButton1, correctCountry.name, true);
            SetButton(answerButton2, wrongCountry.name, false);
        }
        else
        {
            SetButton(answerButton1, wrongCountry.name, false);
            SetButton(answerButton2, correctCountry.name, true);
        }
    }

    void SetButton(Button button, string text, bool isCorrect)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnAnswerSelected(button, isCorrect));
    }

    void OnAnswerSelected(Button button, bool isCorrect)
    {
        SetButtonsInteractable(false);

        if (isCorrect)
        {
            score += 100;
            button.image.color = Color.green;
            SoundManager.Instance.PlayCorrect();
        }
        else
        {
            button.image.color = Color.red;
            SoundManager.Instance.PlayWrong();
        }

        scoreText.text = "Score: " + score;
        Invoke(nameof(ResetButtonsAndShowNext), 0.5f);
    }

    void ResetButtonsAndShowNext()
    {
        answerButton1.image.color = Color.white;
        answerButton2.image.color = Color.white;
        ShowNewFlag();
        SetButtonsInteractable(true);
    }

    void EndGame()
    {
        isGameActive = false;
        timeUpPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        StartCoroutine(GameAPI.Instance.PostPlayHistory(score,
                    onSuccess: () => {
                        Debug.Log("Score posted successfully.");
                    },
                    onError: (error) => {
                        Debug.LogError($"Failed to post score: {error}");
                    }));
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //counter spam
    void SetButtonsInteractable(bool interactable)
    {
        answerButton1.interactable = interactable;
        answerButton2.interactable = interactable;
    }

}
