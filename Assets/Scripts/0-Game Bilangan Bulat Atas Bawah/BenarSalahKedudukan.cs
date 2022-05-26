using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BenarSalahKedudukan : MonoBehaviour
{
    [System.Serializable]
    public struct DetailPertanyaan
    {
        public string pertanyaan;
        public bool jawaban;
        public AudioClip audioClip;
    }

    public bool isPlay;
    public bool isCorrect;
    public bool isAnswered;
    public int totalChallenge;
    public AtasBawahManager gameManager;
    public List<DetailPertanyaan> pertanyaanList;
    public List<GameObject> notifikasiGroup;

    [Header("Element Attribute")]
    public GameObject benarButton;
    public GameObject salahButton;
    public TextMeshProUGUI pertanyaanText;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalChallenge = pertanyaanList.Count;
        
        if (totalChallenge != 0 && gameManager.isPlay)
        {
            benarButton.GetComponent<Button>().interactable = salahButton.GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;

            if (!isPlay && !gameObject.GetComponent<AudioSource>().isPlaying)
            {
                SetupQuestion();
                isPlay = true;
            }
            else if (isPlay && isAnswered && notifikasiGroup[0].transform.localScale.x <= 0)
            {
                if (isCorrect)
                {
                    notifikasiGroup[1].GetComponent<Animator>().SetTrigger("isAnimate");
                    gameObject.GetComponent<AudioSource>().clip = correctSFX;
                }
                else
                {
                    notifikasiGroup[2].GetComponent<Animator>().SetTrigger("isAnimate");
                    gameObject.GetComponent<AudioSource>().clip = wrongSFX;
                }

                if (!gameObject.GetComponent<AudioSource>().isPlaying)
                    gameObject.GetComponent<AudioSource>().Play();
                else
                {
                    isPlay = isAnswered = false;

                    if (isCorrect)
                        pertanyaanList.RemoveAt(0);

                    notifikasiGroup[1].GetComponent<Animator>().ResetTrigger("isAnimate");
                    notifikasiGroup[2].GetComponent<Animator>().ResetTrigger("isAnimate");
                }
            }
        }

        if (totalChallenge == 0 && !gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<Animator>().SetTrigger("isScaleDown");
            if (gameObject.transform.localScale.x <= 0)
            {
                gameManager.gameIndex++;
                gameObject.SetActive(false);
                gameManager.isPlay = false;
            }
        }
    }

    public void SetupQuestion()
    {
        pertanyaanText.text = "...";
        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleUp");

        pertanyaanText.text = pertanyaanList[0].pertanyaan;
        gameObject.GetComponent<AudioSource>().clip = pertanyaanList[0].audioClip;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void CheckAnswer(bool answer)
    {
        isAnswered = true;
        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleDown");
        if (answer == pertanyaanList[0].jawaban)
            isCorrect = true;
        else
            isCorrect = false;
    }
}
