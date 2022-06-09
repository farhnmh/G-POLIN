using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PieChartHandler : MonoBehaviour
{
    [System.Serializable]
    public class DiagramBatang
    {
        public string dataName;
        public int dataTotal;
        public int dataAnswer;
        public Color dataColor;
        public List<GameObject> dataButtons;
    };

    public bool isDone;
    public KulinerManager gameManager;
    public AudioClip audioClip;
    public List<GameObject> chartPanel;
    public List<GameObject> chartButton;
    public List<GameObject> exitButton;
    public List<GameObject> otherButton;
    public List<DiagramBatang> diagramDetail;
    public AudioClip[] notifSFX;
    public GameObject[] notifObject;

    // Start is called before the first frame update
    void Start()
    {
        if (audioClip != null)
        {
            gameObject.GetComponent<AudioSource>().clip = audioClip;
            gameObject.GetComponent<AudioSource>().Play();
        }

        for (int i = 0; i < diagramDetail.Count; i++)
            diagramDetail[i].dataButtons[0].GetComponent<Image>().color = diagramDetail[i].dataColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone && gameObject.transform.localScale.x <= 0)
            gameObject.SetActive(false);

        for (int i = 0; i < chartButton.Count; i++)
            chartButton[i].GetComponent<Button>().interactable = !gameObject.GetComponent<AudioSource>().isPlaying;

        for (int i = 0; i < chartPanel.Count; i++)
        {
            for (int j = 0; j < diagramDetail.Count; j++)
            {
                for (int k = 0; k < diagramDetail[j].dataButtons.Count; k++)
                {
                    if (chartPanel[i].transform.localScale.x >= 1)
                    {
                        exitButton[i].GetComponent<Button>().interactable = true;
                        diagramDetail[j].dataButtons[k].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        exitButton[i].GetComponent<Button>().interactable = false;
                        diagramDetail[j].dataButtons[k].GetComponent<Button>().interactable = false;
                    }
                }
            }
        }
    }

    public void OpenChartPanel(int index)
    {
        if (index == 0)
        {
            chartPanel[0].GetComponent<Animator>().SetTrigger("isScaleUp");
            chartPanel[1].GetComponent<Animator>().SetTrigger("isHide");
        }
        else
        {
            chartPanel[1].GetComponent<Animator>().SetTrigger("isScaleUp");
            chartPanel[0].GetComponent<Animator>().SetTrigger("isHide");

            otherButton[0].SetActive(true);
            otherButton[1].SetActive(true);
        }
    }

    public void CloseChartPanel(int index)
    {
        if (index == 0)
        {
            chartPanel[0].GetComponent<Animator>().SetTrigger("isScaleDown");
            chartPanel[1].GetComponent<Animator>().SetTrigger("isAppear");
        }
        else
        {
            chartPanel[1].GetComponent<Animator>().SetTrigger("isScaleDown");
            chartPanel[0].GetComponent<Animator>().SetTrigger("isAppear");

            otherButton[0].SetActive(false);
            otherButton[1].SetActive(false);
        }
    }

    public void SetupLogDiagram(GameObject button)
    {
        for (int i = 0; i < diagramDetail.Count; i++)
        {
            if (button.transform.parent.name == diagramDetail[i].dataName)
            {
                string temp = button.name;
                temp = temp.Replace("Button (", "");
                temp = temp.Replace(")", "");

                for (int j = 0; j < diagramDetail[i].dataButtons.Count; j++)
                {
                    diagramDetail[i].dataAnswer = int.Parse(temp);

                    if (diagramDetail[i].dataAnswer > j)
                        diagramDetail[i].dataButtons[j].GetComponent<Image>().color = diagramDetail[i].dataColor;
                    else
                        diagramDetail[i].dataButtons[j].GetComponent<Image>().color = Color.white;
                }
            }
        }
    }

    public void ResetAnswer()
    {
        for (int i = 0; i < diagramDetail.Count; i++)
        {
            for (int j = 0; j < diagramDetail[i].dataButtons.Count; j++)
            {
                diagramDetail[i].dataAnswer = 0;
                diagramDetail[i].dataButtons[j].GetComponent<Image>().color = Color.white;
            }

            diagramDetail[i].dataButtons[0].GetComponent<Image>().color = diagramDetail[i].dataColor;
        }
    }

    public void CheckAnswer()
    {
        int checker = 0;
        for (int i = 0; i < diagramDetail.Count; i++)
            if (diagramDetail[i].dataTotal != diagramDetail[i].dataAnswer) checker++;

        if (checker == 0) isDone = true;
        else isDone = false;

        StartCoroutine(WaitingForNotification(isDone));
    }

    public IEnumerator WaitingForNotification(bool condition)
    {
        if (condition)
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
            gameManager.gameIndex++;
            gameManager.isPlay = false;
            gameObject.GetComponent<Animator>().SetTrigger("isScaleDown");
        }
    }
}