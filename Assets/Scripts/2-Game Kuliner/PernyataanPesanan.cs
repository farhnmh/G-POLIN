using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PernyataanPesanan : MonoBehaviour
{
    [System.Serializable]
    public class DetailPernyataan
    {
        public bool isCorrect;
        public GameObject buttonPernyataan;

        [TextArea(3, 10)]
        public string isiPernyataan;
    };

    public bool isDone;
    [TextArea(3, 10)]
    public string isiPernyataan;
    public TextMeshProUGUI pertanyaanText;
    public BelanjaKarakterHandler gameManager;
    public AudioClip audioClip;
    public List<Color> colorButton;
    public List<DetailPernyataan> pernyataanList;
    public List<GameObject> buttons;
    public AudioClip[] notifSFX;
    public GameObject[] notifObject;

    // Start is called before the first frame update
    void Start()
    {
        pertanyaanText.text = isiPernyataan;
        gameObject.GetComponent<AudioSource>().clip = audioClip;
        gameObject.GetComponent<AudioSource>().Play();

        for (int i = 0; i < pernyataanList.Count; i++)
            pernyataanList[i].buttonPernyataan.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pernyataanList[i].isiPernyataan;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;

        for (int i = 0; i < pernyataanList.Count; i++)
            pernyataanList[i].buttonPernyataan.GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
    }

    public void SetupButtonPernyataan(GameObject button)
    {
        if (button.GetComponent<Image>().color == colorButton[0])
            button.GetComponent<Image>().color = colorButton[1];
        else
            button.GetComponent<Image>().color = colorButton[0];
    }

    public void ResetPernyataan()
    {
        for (int i = 0; i < pernyataanList.Count; i++)
            pernyataanList[i].buttonPernyataan.GetComponent<Image>().color = colorButton[0];
    }

    public void JawabPernyataan()
    {
        int checker = 0;
        for (int i = 0; i < pernyataanList.Count; i++)
        {
            if (pernyataanList[i].buttonPernyataan.GetComponent<Image>().color == colorButton[0] &&
                pernyataanList[i].isCorrect) checker++;
            else if (pernyataanList[i].buttonPernyataan.GetComponent<Image>().color == colorButton[1] &&
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
