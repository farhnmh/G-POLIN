using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct GamePanelDetail
    {
        public GameObject transitionPanel;
        public GameObject gamePanel;
    };

    public bool isPlay;
    public bool isTransition;
    public int panelIndex;
    public GameObject finishPanel;
    public List<GamePanelDetail> gamePanelDetail;

    void Update()
    {
        if (panelIndex != gamePanelDetail.Count)
        {
            if (!isTransition && !isPlay)
            {
                if (!gamePanelDetail[panelIndex].transitionPanel.activeInHierarchy)
                {
                    gamePanelDetail[panelIndex].transitionPanel.SetActive(true);
                    isTransition = true;
                }
            }
            else if (isTransition && gamePanelDetail[panelIndex].transitionPanel.GetComponent<TransitionMonologueBox>().isDone)
            {
                gamePanelDetail[panelIndex].gamePanel.SetActive(true);
                if (gamePanelDetail[panelIndex].transitionPanel.GetComponent<TransitionMonologueBox>().isDone)
                {
                    gamePanelDetail[panelIndex].transitionPanel.SetActive(false);
                    isTransition = false;
                    isPlay = true;
                }
            }
        }
        else
        {
            finishPanel.SetActive(true);
            finishPanel.GetComponent<Animator>().SetTrigger("isScaleUp");
        }
    }
}
