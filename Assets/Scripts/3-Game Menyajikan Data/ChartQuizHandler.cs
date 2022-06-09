using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChartQuizHandler : MonoBehaviour
{
    public bool isDone;
    public int gameIndex;
    public KulinerManager gameManager;
    public AudioClip[] questionSFX;
    public AudioClip[] notifSFX;
    public GameObject[] notifObject;

    [Header("Pertanyaan Pertama")]
    public int colorBoxCorrectAnswer;
    public TMP_Dropdown colorBoxDropdown;
    public TMP_Dropdown whiteBoxDropdown;
    public Button firstJawabButton;

    [Header("Pertanyaan Kedua")]
    public int boxQuestionIndex;
    public int boxQuestionCorrectAnswer;
    public List<TMP_Dropdown> boxQuestion;
    public Button secondJawabButton;

    [Header("Pertanyaan Ketiga")]
    public int pieChartCorrect;
    public List<GameObject> pieChartButton;

    // Start is called before the first frame update
    void Start()
    {
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        SetupButton();

        if (isDone && gameObject.transform.localScale.x <= 0)
            gameObject.SetActive(false);
    }

    public void SetupButton()
    {
        //pertanyaan pertama
        if (gameIndex == 0) 
        {
            colorBoxDropdown.interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
            whiteBoxDropdown.interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
            firstJawabButton.interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
        }
        else
        {
            colorBoxDropdown.interactable = false;
            whiteBoxDropdown.interactable = false;
            firstJawabButton.interactable = false;
        }

        //pertanyaan kedua
        for (int i = 0; i < boxQuestion.Count; i++)
        {
            if (i == boxQuestionIndex && gameIndex == 1) boxQuestion[i].interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
            else boxQuestion[i].interactable = false;
        }

        if (gameIndex == 1) secondJawabButton.interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
        else secondJawabButton.interactable = false;

        //pertanyaan ketiga
        for (int i = 0; i < pieChartButton.Count; i++)
        {
            if (gameIndex == 2)
                pieChartButton[i].transform.GetChild(0).GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
            else
                pieChartButton[i].transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
    }

    public void SetupGame()
    {
        gameObject.GetComponent<AudioSource>().clip = questionSFX[gameIndex];
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void SetupAnswer(int answer)
    {
        bool condition = false;
        if (gameIndex == 0)
        {
            int answer1 = int.Parse(colorBoxDropdown.options[colorBoxDropdown.value].text);
            int answer2 = int.Parse(whiteBoxDropdown.options[whiteBoxDropdown.value].text);

            if (answer1 == colorBoxCorrectAnswer && answer2 == 10 - answer1) condition = true;
            else condition = false;
        }
        else if (gameIndex == 1)
        {
            if (int.Parse(boxQuestion[boxQuestionIndex].options[boxQuestion[boxQuestionIndex].value].text.Replace("%", "")) == boxQuestionCorrectAnswer) condition = true;
        }
        else if (gameIndex == 2)
        {
            if (answer == pieChartCorrect) condition = true;
        }

        StartCoroutine(WaitingForNotification(condition));
    }

    public IEnumerator WaitingForNotification(bool condition)
    {
        if (condition)
        {
            notifObject[0].GetComponent<Animator>().SetTrigger("isAnimate");
            gameObject.GetComponent<AudioSource>().clip = notifSFX[0];
        }
        else
        {
            notifObject[1].GetComponent<Animator>().SetTrigger("isAnimate");
            gameObject.GetComponent<AudioSource>().clip = notifSFX[1];
        }

        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitUntil(() => !gameObject.GetComponent<AudioSource>().isPlaying);

        if (condition)
        {
            if (gameIndex == 2)
            {
                isDone = true;
                gameManager.gameIndex++;
                gameManager.isPlay = false;
                gameObject.GetComponent<Animator>().SetTrigger("isScaleDown");
            }
            else
            {
                gameIndex++;
                SetupGame();
            }
        }
    }
}
