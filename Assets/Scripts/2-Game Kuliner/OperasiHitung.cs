using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OperasiHitung : MonoBehaviour
{
    [System.Serializable]
    public class DetailPernyataan
    {
        public bool isCorrect;
        public GameObject buttonPernyataan;
    };

    public bool isDone;
    [TextArea(3, 10)]
    public string isiPernyataan;
    public TextMeshProUGUI pertanyaanText;
    public BelanjaKarakterHandler gameManager;
    public AudioClip audioClip;
    public List<DetailPernyataan> pernyataanList;
    public AudioClip[] notifSFX;
    public GameObject[] notifObject;

    // Start is called before the first frame update
    void Start()
    {
        pertanyaanText.text = isiPernyataan;
        gameObject.GetComponent<AudioSource>().clip = audioClip;
        gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pernyataanList.Count; i++)
            pernyataanList[i].buttonPernyataan.GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
    }

    public void JawabPernyataan(GameObject button)
    {
        int checker = 0;
        for (int i = 0; i < pernyataanList.Count; i++)
        {
            if (button == pernyataanList[i].buttonPernyataan &&
                !pernyataanList[i].isCorrect) checker++;
        }

        if (checker == 0) isDone = true;
        else isDone = false;

        StartCoroutine(WaitingForNotification());
    }

    public void JawabPilihanGanda(GameObject button)
    {
        int checker = 0;
        for (int i = 0; i < pernyataanList.Count; i++)
        {
            if (button == pernyataanList[i].buttonPernyataan &&
                !pernyataanList[i].isCorrect) checker++;
        }

        if (checker == 0) isDone = true;
        else isDone = false;

        StartCoroutine(WaitingForNotification());
    }

    public IEnumerator WaitingForNotification()
    {
        if (isDone)
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

        if (isDone)
        {
            gameObject.GetComponent<Animator>().SetTrigger("isScaleDown");
            gameManager.isPlay = false;
            gameManager.gameIndex++;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().clip = audioClip;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
