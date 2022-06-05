using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OrderMakanan : MonoBehaviour
{
    [System.Serializable]
    public class DetailPesanan
    {
        public Image namaMakanan;
        public int hargaMakanan;
        public int jumlahPesanan;
    };

    public bool isDone;
    public string name;
    public int uangYangDibawa;
    public int totalHarga;
    
    [TextArea(3, 10)]
    public string pertanyaan;
    public KesukaanKarakter kesukaanKarakter;
    public BelanjaKarakterHandler gameManager;
    public AudioClip audioClip;
    public TextMeshProUGUI pertanyaanText;
    public TextMeshProUGUI uangYangDibawaText;
    public TextMeshProUGUI totalHargaText;
    public GameObject mangkok;
    public Vector2 batasMin;
    public Vector2 batasMax;
    public List<DetailPesanan> detailMakanan;
    public List<GameObject> buttons;
    public AudioClip[] notifSFX;
    public GameObject[] notifObject;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().clip = audioClip;
        gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        pertanyaanText.text = pertanyaan;
        uangYangDibawaText.text = $"Uang Yang Dibawa: Rp {uangYangDibawa}";
        totalHargaText.text = $"Total Harga: Rp {totalHarga}";

        for (int i = 0; i < buttons.Count; i++)
            buttons[i].GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;

        for (int i = 0; i < detailMakanan.Count; i++)
        {
            if (isDone) detailMakanan[i].namaMakanan.transform.parent.GetComponent<Button>().interactable = false;
            else detailMakanan[i].namaMakanan.transform.parent.GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;
        }
    }

    public void SpawnMakanan(int index)
    {
        int totalTemp = totalHarga;
        totalTemp += detailMakanan[index].hargaMakanan;

        if (uangYangDibawa >= totalTemp) 
        {
            Vector3 newPos = new Vector3(Random.Range(batasMin.x, batasMax.x), Random.Range(batasMin.y, batasMax.y), 0);
            
            var makanan = Instantiate(detailMakanan[index].namaMakanan);
            makanan.transform.parent = mangkok.transform;
            makanan.transform.localScale = new Vector3(1, 1, 1);
            makanan.GetComponent<RectTransform>().anchoredPosition = newPos;

            totalHarga += detailMakanan[index].hargaMakanan;
            detailMakanan[index].jumlahPesanan++;
        }
    }

    public void ResetPesanan()
    {
        totalHarga = 0;
        for (int i = 0; i < detailMakanan.Count; i++)
            detailMakanan[i].jumlahPesanan = 0;

        foreach (Transform child in mangkok.transform)
            Destroy(child.gameObject);
    }

    public void JawabPesanan()
    {
        int checker = 0;

        for (int i = 0; i < detailMakanan.Count; i++)
        {
            for (int j = 0; j < kesukaanKarakter.characterFavourites.Count; j++)
            {
                if (name == kesukaanKarakter.characterFavourites[j].name)
                {
                    for (int k = 0; k < kesukaanKarakter.characterFavourites[j].favouriteFoods.Count; k++)
                    {
                        if (detailMakanan[i].namaMakanan.name == kesukaanKarakter.characterFavourites[j].favouriteFoods[k].foodName)
                        {
                            if (detailMakanan[i].jumlahPesanan != kesukaanKarakter.characterFavourites[j].favouriteFoods[k].favouriteValue)
                            {
                                checker++;
                                break; 
                            }
                        }
                    }
                }
            }
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
            ResetPesanan();
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