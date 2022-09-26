using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvaluasiManager : MonoBehaviour
{
    public bool isPlay;
    public int panelIndex;
    public int scorePlayer;
    public int scoreResult;
    public GameObject finishPanel;
    public TextMeshProUGUI scoreText;
    public List<GameObject> evaluasiPanel;

    // Update is called once per frame
    void Update()
    {
        scoreResult = scorePlayer * 2;
        scoreText.text = $"SKORMU ADALAH {scoreResult}";

        if (panelIndex < evaluasiPanel.Count)
        {
            if (!isPlay)
            {
                evaluasiPanel[panelIndex].SetActive(true);
                evaluasiPanel[panelIndex].GetComponent<Animator>().SetTrigger("isScaleUp");
                isPlay = true;
            }
        }
        else
        {
            finishPanel.SetActive(true);
            finishPanel.GetComponent<Animator>().SetTrigger("isScaleUp");
        }
    }
}
