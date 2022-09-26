using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EvaluasiKuliner : MonoBehaviour
{
    [System.Serializable]
    public class ButtonDetail
    {
        public bool isCorrect;
        public Button buttonObject;
    };

    [System.Serializable]
    public class PertanyaanDetail
    {
        public GameObject pertanyaanObject;
        public AudioClip pertanyaanAudio;
        public int scoreFactor;
        public List<ButtonDetail> buttonDetails;
    };

    public bool isPlay;
    public bool isDone;
    public int pertanyaanIndex;
    public EvaluasiManager evaluasiManager;
    public AudioSource audioSource;
    public AudioClip correctAudio;
    public AudioClip incorrectAudio;
    public GameObject correctNotif;
    public GameObject incorrectNotif;
    public List<Color> colorButton;
    public List<PertanyaanDetail> pertanyaanDetails;

    // Update is called once per frame
    void Update()
    {
        if (!isDone)
        {
            if (!isPlay && pertanyaanIndex == pertanyaanDetails.Count)
            {
                gameObject.GetComponent<Animator>().SetTrigger("isScaleDown");

                evaluasiManager.panelIndex++;
                evaluasiManager.isPlay = false;
                isDone = true;
            }
            else if (!isPlay)
            {
                audioSource.clip = pertanyaanDetails[pertanyaanIndex].pertanyaanAudio;
                audioSource.Play();

                pertanyaanDetails[pertanyaanIndex].pertanyaanObject.SetActive(true);
                pertanyaanDetails[pertanyaanIndex].pertanyaanObject.GetComponent<Animator>().SetTrigger("isScaleUp");

                pertanyaanIndex++;
                isPlay = true;
            }
            else
                for (int i = 0; i < pertanyaanDetails[pertanyaanIndex - 1].buttonDetails.Count; i++)
                    pertanyaanDetails[pertanyaanIndex - 1].buttonDetails[i].buttonObject.interactable = !audioSource.isPlaying;
        }
    }

    public void ResetButtonColor()
    {
        for (int i = 0; i < pertanyaanDetails[0].buttonDetails.Count; i++)
            pertanyaanDetails[0].buttonDetails[i].buttonObject.image.color = colorButton[1];
    }

    public void ChangeButtonColor(Button button)
    {
        if (button.image.color == colorButton[0])
            button.image.color = colorButton[1];
        else
            button.image.color = colorButton[0];
    }

    public void SubmitAnswer()
    {
        int checker = 0;
        for (int i = 0; i < pertanyaanDetails[0].buttonDetails.Count; i++)
            if (pertanyaanDetails[0].buttonDetails[i].buttonObject.image.color == colorButton[0] && pertanyaanDetails[0].buttonDetails[i].isCorrect) checker++;

        if (checker != 0) StartCoroutine(PopUpNotification(correctNotif, correctAudio));
        else StartCoroutine(PopUpNotification(incorrectNotif, incorrectAudio));
        evaluasiManager.scorePlayer += (checker * 5);
    }

    public void ChooseAnswer(Button button)
    {
        int checker = 0;
        for (int i = 0; i < pertanyaanDetails[pertanyaanIndex - 1].buttonDetails.Count; i++)
            if (pertanyaanDetails[pertanyaanIndex - 1].buttonDetails[i].buttonObject == button && pertanyaanDetails[pertanyaanIndex - 1].buttonDetails[i].isCorrect) 
            {
                evaluasiManager.scorePlayer += pertanyaanDetails[pertanyaanIndex - 1].scoreFactor;
                checker++;
            }

        if (checker != 0) StartCoroutine(PopUpNotification(correctNotif, correctAudio));
        else StartCoroutine(PopUpNotification(incorrectNotif, incorrectAudio));
    }

    public IEnumerator PopUpNotification(GameObject notif, AudioClip clip)
    {
        notif.GetComponent<Animator>().SetTrigger("isAnimate");
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitUntil(() => !audioSource.isPlaying);
        pertanyaanDetails[pertanyaanIndex - 1].pertanyaanObject.GetComponent<Animator>().SetTrigger("isScaleDown");
        isPlay = false;
    }
}
