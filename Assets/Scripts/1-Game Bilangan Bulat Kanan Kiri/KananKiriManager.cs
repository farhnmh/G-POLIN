using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class KananKiriManager : MonoBehaviour
{
    [System.Serializable]
    public class GameDetail
    {
        public int targetKedudukan;
        public int objectDibeli;
        public string posisiKedudukan;
        public string nilaiKedudukan;
        public List<AudioClip> audioClip;
        public List<string> soalDitanyakan;
        public List<Sprite> multipleButtonChoice;
    };

    public bool isPlay;
    public bool isChosen;
    public bool isCorrect;
    public int gameIndex;
    public int questionIndex;
    public GameManager gameManager;
    public GameObject transitionBefore;
    public List<GameDetail> gameDetails;

    [Header("Elements Attribute")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;
    public TextMeshProUGUI pertanyaanPertama;
    public TextMeshProUGUI pertanyaanKedua;
    public List<Sprite> buttonType;
    public List<GameObject> questionGroup;
    public List<GameObject> notifikasiGroup;
    public List<GameObject> tokoTujuanButton;
    public List<GameObject> objectDibeliButton;
    public List<GameObject> nilaiKedudukanDropdown;

    void Awake()
    {
        for (int i = 0; i < gameManager.gamePanelDetail.Count; i++)
            if (gameObject == gameManager.gamePanelDetail[i].gamePanel)
                transitionBefore = gameManager.gamePanelDetail[i].transitionPanel;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDetails.Count != 0)
        {
            SetupButton();
            if (!transitionBefore.activeInHierarchy && !gameObject.GetComponent<AudioSource>().isPlaying && !isPlay)
                SetupQuestion();
            else if (isPlay && isChosen && notifikasiGroup[0].transform.localScale.x <= 0)
            {
                if (isCorrect)
                {
                    notifikasiGroup[1].GetComponent<Animator>().SetTrigger("isAnimate");
                    gameObject.GetComponent<AudioSource>().clip = correctSFX;
                    gameObject.GetComponent<AudioSource>().Play();

                    if (gameDetails[gameIndex].soalDitanyakan.Count == questionIndex + 1)
                    {
                        gameDetails.RemoveAt(0);
                        questionIndex = 0;
                    }
                    else
                        questionIndex++;
                }
                else
                {
                    notifikasiGroup[2].GetComponent<Animator>().SetTrigger("isAnimate");
                    gameObject.GetComponent<AudioSource>().clip = wrongSFX;
                    gameObject.GetComponent<AudioSource>().Play();
                }

                isCorrect = isPlay = isChosen = false;
            }
        }
        else if (gameDetails.Count == 0 && !gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameManager.panelIndex++;
            gameManager.isPlay = false;
            gameObject.SetActive(false);
        }
    }

    public void SetupButton()
    {
        if (questionIndex == 0)
        {
            for (int i = 0; i < tokoTujuanButton.Count; i++)
                tokoTujuanButton[i].transform.GetChild(0).GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
            for (int i = 0; i < objectDibeliButton.Count; i++)
                objectDibeliButton[i].GetComponent<Button>().interactable = false;
            for (int i = 0; i < nilaiKedudukanDropdown.Count; i++)
                nilaiKedudukanDropdown[i].GetComponent<TMP_Dropdown>().interactable = false;
        }
        else if (questionIndex == 1)
        {
            for (int i = 0; i < tokoTujuanButton.Count; i++)
                tokoTujuanButton[i].transform.GetChild(0).GetComponent<Button>().interactable = false;
            for (int i = 0; i < objectDibeliButton.Count; i++)
                objectDibeliButton[i].GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
            for (int i = 0; i < nilaiKedudukanDropdown.Count; i++)
                nilaiKedudukanDropdown[i].GetComponent<TMP_Dropdown>().interactable = false;
        }
        else if (questionIndex == 2)
        {
            for (int i = 0; i < tokoTujuanButton.Count; i++)
                tokoTujuanButton[i].transform.GetChild(0).GetComponent<Button>().interactable = false;
            for (int i = 0; i < objectDibeliButton.Count; i++)
                objectDibeliButton[i].GetComponent<Button>().interactable = false;
            for (int i = 0; i < nilaiKedudukanDropdown.Count; i++)
                nilaiKedudukanDropdown[i].GetComponent<TMP_Dropdown>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
        }
    }

    public void SetupQuestion()
    {
        if (questionIndex == 0)
            for (int i = 0; i < tokoTujuanButton.Count; i++)
                tokoTujuanButton[i].GetComponent<Image>().sprite = buttonType[0];

        for (int i = 0; i < questionGroup.Count; i++)
        {
            questionGroup[i].GetComponent<Animator>().ResetTrigger("isScaleUp");
            questionGroup[i].GetComponent<Animator>().ResetTrigger("isScaleDown");

            if (i == questionIndex)
                questionGroup[i].GetComponent<Animator>().SetTrigger("isScaleUp");
            else
                questionGroup[i].GetComponent<Animator>().SetTrigger("isScaleDown");
        }

        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleUp");
        gameObject.GetComponent<AudioSource>().PlayOneShot(gameDetails[gameIndex].audioClip[questionIndex]);
        
        pertanyaanPertama.text = gameDetails[gameIndex].soalDitanyakan[0];
        pertanyaanKedua.text = gameDetails[gameIndex].soalDitanyakan[1];

        for (int i = 0; i < objectDibeliButton.Count; i++)
            objectDibeliButton[i].GetComponent<Image>().sprite = gameDetails[gameIndex].multipleButtonChoice[i];

        isPlay = true;
    }

    public void CheckAnswer(GameObject button)
    {
        isChosen = true;
        int answerTemp = 0;

        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleDown");
        for (int i = 0; i < gameDetails.Count; i++)
        {
            if (i == gameIndex)
            {
                if (questionIndex == 0)
                {
                    for (int j = 0; j < tokoTujuanButton.Count; j++)
                    {
                        if (button.transform.parent.gameObject == tokoTujuanButton[j])
                        {
                            button.transform.parent.GetComponent<Image>().sprite = buttonType[1];
                            answerTemp = j - 5;
                        }
                    }

                    if (answerTemp == gameDetails[gameIndex].targetKedudukan) isCorrect = true;
                    else isCorrect = false;
                }
                else if (questionIndex == 1)
                {
                    for (int j = 0; j < objectDibeliButton.Count; j++)
                        if (button == objectDibeliButton[j]) answerTemp = j + 1;

                    if (answerTemp == gameDetails[gameIndex].objectDibeli) isCorrect = true;
                    else isCorrect = false;
                }
                else if (questionIndex == 2)
                {
                    string posisi = nilaiKedudukanDropdown[0].GetComponent<TMP_Dropdown>().options[nilaiKedudukanDropdown[0].GetComponent<TMP_Dropdown>().value].text;
                    string nilai = nilaiKedudukanDropdown[1].GetComponent<TMP_Dropdown>().options[nilaiKedudukanDropdown[1].GetComponent<TMP_Dropdown>().value].text;

                    if (posisi == gameDetails[gameIndex].posisiKedudukan &&
                        nilai == gameDetails[gameIndex].nilaiKedudukan) isCorrect = true;
                    else isCorrect = false;
                }

                break;
            }
        }
    }
}