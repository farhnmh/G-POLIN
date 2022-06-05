using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelanjaKarakterHandler : MonoBehaviour
{
    public bool isPlay;
    public bool isDone;
    public int gameIndex;
    public KulinerManager gameManager;
    public List<GameObject> gamePanel;

    // Update is called once per frame
    void Update()
    {
        if (!isPlay)
        {
            if (gameIndex == gamePanel.Count)
            {
                gameManager.gameIndex++;
                gameManager.isPlay = false;
                gameObject.SetActive(false);
            }

            gamePanel[gameIndex].SetActive(true);
            gamePanel[gameIndex].GetComponent<Animator>().SetTrigger("isScaleUp");
            isPlay = true;
        }
    }
}
