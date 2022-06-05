using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RefleksiKedudukan : MonoBehaviour
{
    [System.Serializable]
    public class DetailRefleksi
    {
        public string refleksiAnswer;
        public TMP_Dropdown refleksiDropdown;
    };

    public bool isPlay;
    public bool isChosen;
    public bool isCorrect;
    public string refleksiAtasJawaban;
    public string refleksiBawahJawaban;
    public AtasBawahManager gameManager;
    public GameObject jawabButton;
    public AudioClip soalSFX;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;
    public List<GameObject> notifikasiGroup;
    public List<DetailRefleksi> detailRefleksi;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isPlay)
        {
            jawabButton.GetComponent<Button>().interactable =
                detailRefleksi[0].refleksiDropdown.interactable =
                detailRefleksi[1].refleksiDropdown.interactable = 
                !gameObject.GetComponent<AudioSource>().isPlaying;

            if (!isPlay && !gameObject.GetComponent<AudioSource>().isPlaying)
            {
                gameObject.GetComponent<AudioSource>().clip = soalSFX;
                gameObject.GetComponent<AudioSource>().Play();
                isPlay = true;
            }
            else if (isChosen && notifikasiGroup[0].transform.localScale.x <= 0)
            {
                if (isCorrect)
                {
                    notifikasiGroup[1].GetComponent<Animator>().SetTrigger("isAnimate");
                    gameObject.GetComponent<AudioSource>().clip = correctSFX;
                }
                else
                {
                    isPlay = false;
                    notifikasiGroup[2].GetComponent<Animator>().SetTrigger("isAnimate");
                    gameObject.GetComponent<AudioSource>().clip = wrongSFX;
                }

                if (!gameObject.GetComponent<AudioSource>().isPlaying)
                    gameObject.GetComponent<AudioSource>().Play();
                else
                {
                    isChosen = false;
                    notifikasiGroup[1].GetComponent<Animator>().ResetTrigger("isAnimate");
                    notifikasiGroup[2].GetComponent<Animator>().ResetTrigger("isAnimate");
                }
            }
        }

        if (!gameObject.GetComponent<AudioSource>().isPlaying &&
            notifikasiGroup[1].transform.localScale.x <= 0 && 
            isCorrect)
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

    public void CheckAnswer()
    {
        isChosen = true;
        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleDown");
        refleksiAtasJawaban = detailRefleksi[0].refleksiDropdown.options[detailRefleksi[0].refleksiDropdown.value].text;
        refleksiBawahJawaban = detailRefleksi[1].refleksiDropdown.options[detailRefleksi[1].refleksiDropdown.value].text;

        if (detailRefleksi[0].refleksiAnswer == refleksiAtasJawaban &&
            detailRefleksi[1].refleksiAnswer == refleksiBawahJawaban)
            isCorrect = true;
        else
            isCorrect = false;
    }
}
