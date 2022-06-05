using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenentukanNilaiKedudukan : MonoBehaviour
{
    public enum TipeKedudukan
    {
        Diatas,
        Ditengah,
        Dibawah
    };

    [System.Serializable]
    public struct DetailKedudukan
    {
        public string objectName;
        public string nilaiKedudukan;
        public TipeKedudukan tipeKedudukan;
    }

    [System.Serializable]
    public struct DetailPertanyaan
    {
        public string objectName;
        public AudioClip audioClip;
    }

    public bool isPlay;
    public bool isCorrect;
    public bool isChosen;
    public int totalChallenge;
    public AtasBawahManager gameManager;
    public TextMeshProUGUI pertanyaan;
    public TextMeshProUGUI kesimpulan;
    public List<GameObject> notifikasiGroup;
    public List<DetailKedudukan> kedudukanList;
    public List<DetailPertanyaan> detailPertanyaan;

    [Header("Answer Checking Attribute")]
    public int targetAnswer;
    public string nilaiKedudukanPertanyaan;
    public string tipeKedudukanPertanyaan;
    public string nilaiKedudukanJawaban;
    public string tipeKedudukanJawaban;

    [Header("Element Attribute")]
    public GameObject jawabButton;
    public TMP_Dropdown nilaiKedudukanDropdown;
    public TMP_Dropdown tipeKedudukanDropdown;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    // Update is called once per frame
    void Update()
    {
        totalChallenge = detailPertanyaan.Count;

        if (totalChallenge != 0 && gameManager.isPlay)
        {
            jawabButton.GetComponent<Button>().interactable = nilaiKedudukanDropdown.interactable = tipeKedudukanDropdown.interactable = !gameObject.GetComponent<AudioSource>().isPlaying;

            if (!isPlay && !gameObject.GetComponent<AudioSource>().isPlaying)
            {
                SetupQuestion();
                isPlay = true;
            }
            else if (isPlay && isChosen && notifikasiGroup[0].transform.localScale.x <= 0)
            {
                if (isCorrect)
                {
                    kesimpulan.text = $"Kedudukan {detailPertanyaan[0].objectName.ToLower()} berada pada titik {nilaiKedudukanJawaban} {tipeKedudukanJawaban.ToLower()} permukaan laut";
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
                    isPlay = isChosen = false;

                    if (isCorrect)
                        detailPertanyaan.RemoveAt(0);
                    
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
        kesimpulan.text = "...";
        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleUp");

        string questionTemp = detailPertanyaan[0].objectName;
        for (int i = 0; i < kedudukanList.Count; i++)
        {
            if (questionTemp == kedudukanList[i].objectName)
            {
                targetAnswer = i;
                nilaiKedudukanPertanyaan = kedudukanList[i].nilaiKedudukan.ToString();
                tipeKedudukanPertanyaan = kedudukanList[i].tipeKedudukan.ToString();
                pertanyaan.text = $"Kedudukan {kedudukanList[i].objectName}:";
            }
        }
        
        gameObject.GetComponent<AudioSource>().clip = detailPertanyaan[0].audioClip;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void CheckAnswer()
    {
        isChosen = true;
        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleDown");
        nilaiKedudukanJawaban = nilaiKedudukanDropdown.options[nilaiKedudukanDropdown.value].text;
        tipeKedudukanJawaban = tipeKedudukanDropdown.options[tipeKedudukanDropdown.value].text;

        if (nilaiKedudukanJawaban == nilaiKedudukanPertanyaan &&
            tipeKedudukanJawaban == tipeKedudukanPertanyaan)
            isCorrect = true;
        else
            isCorrect = false;
    }
}
