using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class AtasBawahManager : MonoBehaviour
{
    public bool isPlay;
    public bool isDone;
    public int gameIndex;
    public GameManager gameManager;
    public GameObject transitionBefore;
    public List<GameObject> gamePanel;

    void Awake()
    {
        for (int i = 0; i < gameManager.gamePanelDetail.Count; i++)
            if (gameObject == gameManager.gamePanelDetail[i].gamePanel)
                transitionBefore = gameManager.gamePanelDetail[i].transitionPanel;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transitionBefore.activeInHierarchy && !isPlay)
        {
            if (gameIndex == gamePanel.Count)
            {
                gameManager.panelIndex++;
                gameManager.isPlay = false;
                gameObject.SetActive(false);
            }

            gamePanel[gameIndex].SetActive(true);
            gamePanel[gameIndex].GetComponent<Animator>().SetTrigger("isScaleUp");

            if (gamePanel[gameIndex].transform.localScale.x >= 1)
                isPlay = true;
        }
    }
}