using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtasBawahManager : MonoBehaviour
{
    public bool isPlay;
    public int gameIndex;
    public List<GameObject> gamePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlay)
        {
            gamePanel[gameIndex].SetActive(true);
            gamePanel[gameIndex].GetComponent<Animator>().SetTrigger("isScaleUp");

            if (gamePanel[gameIndex].transform.localScale.x >= 1)
                isPlay = true;
        }
        else
        {

        }
    }
}
