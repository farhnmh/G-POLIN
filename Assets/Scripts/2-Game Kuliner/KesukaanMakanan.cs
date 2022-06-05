using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class KesukaanMakanan : MonoBehaviour
{
    [System.Serializable]
    public class KesukaanDetail
    {
        public bool isCorrect;
        public string name;
        public Sprite character;
        public AudioClip audioClip;

        [TextArea(3, 10)]
        public string description;
        public List<GameObject> favouriteFood;
    };

    public int detailIndex;
    public KulinerManager gameManager;
    public List<KesukaanDetail> kesukaanDetail;

    [Header("Element Attributes")]
    public GameObject prevButton;
    public GameObject nextButton;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image characterImage;
    public AudioClip[] notifSFX;
    public GameObject[] notifObject;
    public GameObject[] generalButtons;

    // Start is called before the first frame update
    void Start()
    {
        SetupPetunjuk(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (detailIndex == kesukaanDetail.Count && !gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<Animator>().SetTrigger("isScaleDown");
            if (gameObject.transform.localScale.x <= 0)
            {
                gameManager.gameIndex++;
                gameObject.SetActive(false);
                gameManager.isPlay = false;
            }
        }

        for (int i = 0; i < generalButtons.Length; i++)
            generalButtons[i].GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;

        for (int i = 0; i < kesukaanDetail.Count; i++)
        {
            for (int j = 0; j < kesukaanDetail[i].favouriteFood.Count; j++)
            {
                if (!kesukaanDetail[i].isCorrect)
                {
                    if (i == detailIndex)
                        kesukaanDetail[i].favouriteFood[j].GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
                    else 
                        kesukaanDetail[i].favouriteFood[j].GetComponent<Button>().interactable = false;

                }
                else
                    kesukaanDetail[i].favouriteFood[j].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void SetupPetunjuk(int index)
    {
        if (detailIndex != kesukaanDetail.Count)
        {
            detailIndex += index;

            nameText.text = kesukaanDetail[detailIndex].name;
            descriptionText.text = kesukaanDetail[detailIndex].description;
            characterImage.sprite = kesukaanDetail[detailIndex].character;

            if (kesukaanDetail[detailIndex].audioClip != null)
            {
                gameObject.GetComponent<AudioSource>().clip = kesukaanDetail[detailIndex].audioClip;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    public void ResetKesukaan()
    {
        for (int i = 0; i < kesukaanDetail[detailIndex].favouriteFood.Count; i++)
            kesukaanDetail[detailIndex].favouriteFood[i].GetComponent<MenentukanKesukaan>().favouriteValue = 0;
    }

    public void JawabKesukaan()
    {
        var database = transform.parent.GetComponent<KesukaanKarakter>();

        for (int i = 0; i < kesukaanDetail.Count; i++)
        {
            for (int j = 0; j < database.characterFavourites.Count; j++)
            {
                if (kesukaanDetail[i].name == database.characterFavourites[j].name)
                {
                    for (int k = 0; k < database.characterFavourites[j].favouriteFoods.Count; k++)
                    {
                        if (kesukaanDetail[i].favouriteFood[k].GetComponent<MenentukanKesukaan>().favouriteValue ==
                            database.characterFavourites[j].favouriteFoods[k].favouriteValue)
                        {
                            kesukaanDetail[i].isCorrect = true;
                        }
                        else
                        {
                            kesukaanDetail[i].isCorrect = false;
                            break;
                        }
                    }
                }
            }
        }

        StartCoroutine(WaitingForNotification(kesukaanDetail[detailIndex].isCorrect));
    }

    public IEnumerator WaitingForNotification(bool condition)
    {
        int index = 0;
        if (condition)
        {
            index = 1;
            notifObject[0].GetComponent<Animator>().SetTrigger("isAnimate");
            gameObject.GetComponent<AudioSource>().clip = notifSFX[0];
        }
        else
        {
            index = 0;
            notifObject[1].GetComponent<Animator>().SetTrigger("isAnimate");
            gameObject.GetComponent<AudioSource>().clip = notifSFX[1];
        }

        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitUntil(() => !gameObject.GetComponent<AudioSource>().isPlaying);
        SetupPetunjuk(index);
    }
}
