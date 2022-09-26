using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EvaluasiKananKiri : MonoBehaviour
{
    [System.Serializable]
    public class PertanyaanDetail
    {
        public GameObject pertanyaanObject;
        public AudioClip pertanyaanAudio;
        public List<Button> buttonAnswer;
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
    public TMP_Dropdown kedudukanDropdown;
    public TMP_Dropdown posisiDropdown;
    public Sprite passiveButton;
    public List<PertanyaanDetail> pertanyaanDetail;

    // Update is called once per frame
    void Update()
    {
        if (!isDone)
        {
            if (!isPlay && pertanyaanIndex == pertanyaanDetail.Count)
            {
                gameObject.GetComponent<Animator>().SetTrigger("isScaleDown");

                evaluasiManager.panelIndex++;
                evaluasiManager.isPlay = false;
                isDone = true;
            }
            else if (!isPlay)
            {
                audioSource.clip = pertanyaanDetail[pertanyaanIndex].pertanyaanAudio;
                audioSource.Play();

                pertanyaanDetail[pertanyaanIndex].pertanyaanObject.SetActive(true);
                pertanyaanDetail[pertanyaanIndex].pertanyaanObject.GetComponent<Animator>().SetTrigger("isScaleUp");

                pertanyaanIndex++;
                isPlay = true;
            }
            else
                for (int i = 0; i < pertanyaanDetail[pertanyaanIndex - 1].buttonAnswer.Count; i++)
                    pertanyaanDetail[pertanyaanIndex - 1].buttonAnswer[i].interactable = !audioSource.isPlaying;
        }
    }

    public void ChooseDropdown()
    {
        if (kedudukanDropdown.options[kedudukanDropdown.value].text == "+" &&
            posisiDropdown.options[posisiDropdown.value].text == "6")
        {
            evaluasiManager.scorePlayer += 5;
            StartCoroutine(PopUpNotification(correctNotif, correctAudio));
        }
        else
            StartCoroutine(PopUpNotification(incorrectNotif, incorrectAudio));
    }

    public void ChooseAnswer(bool isCorrect)
    {
        if (isCorrect) { StartCoroutine(PopUpNotification(correctNotif, correctAudio)); evaluasiManager.scorePlayer += 5; }
        else StartCoroutine(PopUpNotification(incorrectNotif, incorrectAudio));
    }

    public void ChangeButtonSprite(Button button)
    {
        button.image.sprite = passiveButton;
    }

    public IEnumerator PopUpNotification(GameObject notif, AudioClip clip)
    {
        for (int i = 0; i < pertanyaanDetail[pertanyaanIndex - 1].buttonAnswer.Count; i++)
            pertanyaanDetail[pertanyaanIndex - 1].buttonAnswer[i].interactable = false;

        pertanyaanDetail[pertanyaanIndex - 1].buttonAnswer.Clear();
        notif.GetComponent<Animator>().SetTrigger("isAnimate");
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitUntil(() => !audioSource.isPlaying);
        pertanyaanDetail[pertanyaanIndex - 1].pertanyaanObject.GetComponent<Animator>().SetTrigger("isScaleDown");
        isPlay = false;
    }
}
