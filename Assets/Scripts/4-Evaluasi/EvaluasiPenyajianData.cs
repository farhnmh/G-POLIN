using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EvaluasiPenyajianData : MonoBehaviour
{
    [System.Serializable]
    public class PertanyaanDetail
    {
        public AudioClip pertanyaanAudio;
        public int scoreFactor;
        public List<Button> buttonDetails;
    };

    public bool isPlay;
    public bool isDone;
    public int pertanyaanIndex;
    public EvaluasiManager evaluasiManager;
    public AudioSource audioSource;
    public AudioClip instructionAudio;
    public AudioClip correctAudio;
    public AudioClip incorrectAudio;
    public GameObject correctNotif;
    public GameObject incorrectNotif;
    public TMP_Dropdown dropdownAnswer;
    public List<PertanyaanDetail> pertanyaanDetails;

    void Start()
    {
        isPlay = true;
        audioSource.PlayOneShot(instructionAudio);
        StartCoroutine(WaitIsPlaying());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDone)
        {
            if (!isPlay && pertanyaanIndex == pertanyaanDetails.Count)
            {
                evaluasiManager.panelIndex++;
                evaluasiManager.isPlay = false;
                isDone = true;
            }
            else if (!isPlay)
            {
                audioSource.clip = pertanyaanDetails[pertanyaanIndex].pertanyaanAudio;
                audioSource.Play();

                pertanyaanIndex++;
                isPlay = true;
            }
            else
            {
                for (int i = 0; i < pertanyaanDetails.Count; i++)
                {
                    for (int j = 0; j < pertanyaanDetails[i].buttonDetails.Count; j++)
                    {
                        if (i == pertanyaanIndex - 1) pertanyaanDetails[i].buttonDetails[j].interactable = !audioSource.isPlaying;
                        else pertanyaanDetails[i].buttonDetails[j].interactable = false;
                    }
                }
            }
        }
    }

    public void ChooseDropdown()
    {
        if (dropdownAnswer.options[dropdownAnswer.value].text == "60%")
        {
            evaluasiManager.scorePlayer += pertanyaanDetails[pertanyaanIndex - 1].scoreFactor;
            StartCoroutine(PopUpNotification(correctNotif, correctAudio));
        }
        else
            StartCoroutine(PopUpNotification(incorrectNotif, incorrectAudio));
    }

    public void ChooseAnswer(bool isCorrect)
    {
        if (isCorrect) 
        { 
            StartCoroutine(PopUpNotification(correctNotif, correctAudio)); 
            evaluasiManager.scorePlayer += pertanyaanDetails[pertanyaanIndex - 1].scoreFactor;
        }
        else 
            StartCoroutine(PopUpNotification(incorrectNotif, incorrectAudio));
    }

    public IEnumerator WaitIsPlaying()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        isPlay = false;
    }

    public IEnumerator PopUpNotification(GameObject notif, AudioClip clip)
    {
        notif.GetComponent<Animator>().SetTrigger("isAnimate");
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitUntil(() => !audioSource.isPlaying);
        isPlay = false;
    }
}
