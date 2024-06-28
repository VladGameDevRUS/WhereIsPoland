using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public Image flagName;
    public Text questionText;
    public Text scoreText;
    public Text finalScore;
    public Button[] replyButtons;
    public Button restartButton;
    public QuizData quizData;
    public GameObject Right;
    public GameObject Wrong;
    public GameObject GameFinished;

    private int currentQuestion = 0;
    private static int score = 0;

    private void Start()
    {
        SetQuestion(currentQuestion);
        Right.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        GameFinished.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    void SetQuestion(int questionIndex)
    {
        questionText.text = quizData.questions[questionIndex].questionText;
        flagName.sprite = quizData.questions[questionIndex].sprite;
        flagName.gameObject.SetActive(true);

        foreach (Button r in replyButtons)
        {
            r.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < replyButtons.Length; i++)
        {
            replyButtons[i].GetComponentInChildren<Text>().text = quizData.questions[questionIndex].replies[i];
            int replyIndex = i;
            replyButtons[i].onClick.AddListener(() =>
            {
                CheckReply(replyIndex);
            });
        }
    }

    void CheckReply(int replyIndex)
    {
        if (replyIndex == quizData.questions[currentQuestion].correctReplyIndex)
        {
            score++;
            scoreText.text = "" + score;
            Right.gameObject.SetActive(true);
        }
        else
        {
            Wrong.gameObject.SetActive(true);
        }

        foreach (Button r in replyButtons)
        {
            r.interactable = false;
        }

        StartCoroutine(Next(replyIndex == quizData.questions[currentQuestion].correctReplyIndex));
    }

    IEnumerator Next(bool isCorrect)
    {
        yield return new WaitForSeconds(2);

        if (isCorrect)
        {
            currentQuestion++;

            if (currentQuestion < quizData.questions.Length)
            {
                Reset();
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            GameOver();
        }
    }

    public void Reset()
    {
        Right.SetActive(false);
        Wrong.SetActive(false);

        foreach (Button r in replyButtons)
        {
            r.interactable = true;
        }

        SetQuestion(currentQuestion);
    }

    public void GameOver()
    {
        GameFinished.SetActive(true);
        restartButton.gameObject.SetActive(true); // Show the restart button

        float scorePercentage = (float)score / quizData.questions.Length * 100;

        finalScore.text = "Твой прогресс:  " + scorePercentage.ToString("F0") + "%";

        if (scorePercentage < 100)
        {
            finalScore.text += "\nПольша не нашлась :(";
        }
        else
        {
            finalScore.text += "\nПоздравляем! Вы нашли Польшу :)";
        }

        flagName.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        score = 0;
        currentQuestion = 0;
        scoreText.text = "" + score;

        Right.SetActive(false);
        Wrong.SetActive(false);
        GameFinished.SetActive(false);
        restartButton.gameObject.SetActive(false);

        foreach (Button r in replyButtons)
        {
            r.interactable = true;
        }

        SetQuestion(currentQuestion);
    }
}
