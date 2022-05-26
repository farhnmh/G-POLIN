using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MemasangkanKedudukan : MonoBehaviour
{
    [System.Serializable]
    public struct DetailKedudukan
    {
        public string pertanyaan;
        public string kedudukan;
        public LineRenderer lineRenderer;
        public Vector2 targetLinePos;
        public AudioClip audioClip;
    }

    public bool isCorrect;
    public bool pertanyaanChosen;
    public bool kedudukanChosen;
    public int totalDone;
    public int listIndex;
    public string objectAnswer;
    public string kedudukanAnswer;
    public AtasBawahManager gameManager;
    public List<DetailKedudukan> kedudukanList;
    public List<GameObject> notifikasiGroup;

    [Header("Element Attribute")]
    public AudioClip correctSFX;
    public AudioClip wrongSFX;
    public List<GameObject> buttonTemp;
    public List<Button> buttonPertanyaan;
    public List<Button> buttonKedudukan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (totalDone < kedudukanList.Count)
        {
            if (!pertanyaanChosen && !kedudukanChosen && !gameObject.GetComponent<AudioSource>().isPlaying)
            {
                for (int i = 0; i < buttonPertanyaan.Count; i++)
                    buttonPertanyaan[i].interactable = true;

                for (int i = 0; i < buttonKedudukan.Count; i++)
                    buttonKedudukan[i].interactable = false;
            }
            else
            {
                if (!pertanyaanChosen)
                    for (int i = 0; i < buttonPertanyaan.Count; i++)
                        buttonPertanyaan[i].interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
                else
                    for (int i = 0; i < buttonPertanyaan.Count; i++)
                        buttonPertanyaan[i].interactable = false;

                if (!kedudukanChosen)
                    for (int i = 0; i < buttonKedudukan.Count; i++)
                        buttonKedudukan[i].interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
                else
                    for (int i = 0; i < buttonKedudukan.Count; i++)
                        buttonKedudukan[i].interactable = false;
            }

            if (pertanyaanChosen && kedudukanChosen && notifikasiGroup[0].transform.localScale.x <= 0)
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
                    listIndex = 0;
                    pertanyaanChosen = kedudukanChosen = false;

                    if (isCorrect)
                        totalDone++;

                    notifikasiGroup[1].GetComponent<Animator>().ResetTrigger("isAnimate");
                    notifikasiGroup[2].GetComponent<Animator>().ResetTrigger("isAnimate");
                }
            }
        }

        if (totalDone == kedudukanList.Count && !gameObject.GetComponent<AudioSource>().isPlaying)
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

    public void SetupObjectAnswer(GameObject child)
    {
        pertanyaanChosen = true;
        var parent = child.transform.parent;
        buttonTemp[0] = child;

        objectAnswer = parent.name;
        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleUp");

        for (int i = 0; i < kedudukanList.Count; i++)
        {
            if (objectAnswer == kedudukanList[i].pertanyaan) {
                listIndex = i;
                gameObject.GetComponent<AudioSource>().clip = kedudukanList[i].audioClip;
                gameObject.GetComponent<AudioSource>().Play();
            } 
        }
    }

    public void SetupKedudukanAnswer(GameObject child)
    {
        kedudukanChosen = true;
        var parent = child.transform.parent;
        buttonTemp[1] = child;

        kedudukanAnswer = parent.name;
        notifikasiGroup[0].GetComponent<Animator>().SetTrigger("isScaleDown");

        if (objectAnswer == kedudukanList[listIndex].pertanyaan &&
            kedudukanAnswer == kedudukanList[listIndex].kedudukan)
        {
            kedudukanList[listIndex].lineRenderer.SetPosition(1, kedudukanList[listIndex].targetLinePos);

            for (int i = 0; i < buttonPertanyaan.Count; i++)
            {
                if (buttonTemp[0].GetComponent<Button>() == buttonPertanyaan[i])
                {
                    buttonTemp[0] = null;
                    buttonPertanyaan[i].interactable = false;
                    buttonPertanyaan.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < buttonKedudukan.Count; i++)
            {
                if (buttonTemp[1].GetComponent<Button>() == buttonKedudukan[i])
                {
                    buttonTemp[1] = null;
                    buttonKedudukan[i].interactable = false;
                    buttonKedudukan.RemoveAt(i);
                    break;
                }
            }

            isCorrect = true;
        }
        else
            isCorrect = false;
    }
}
