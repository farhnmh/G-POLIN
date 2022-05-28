using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class TransitionMonologueBox : MonoBehaviour
{
    public bool isRun;
    public bool isDone;
    public float delayFactor;
    public float moveSpeed;
    public GameObject nextButton;
    public GameObject carObject;
    public AnimationClip carAnimation;
    public List<GameObject> targetPos;

    void Awake()
    {
        GetComponent<Animator>().SetTrigger("isScaleUp");
        StartCoroutine(ShowButton());
    }

    void Update()
    {
        nextButton.GetComponent<Button>().onClick.AddListener(OnClickButton);

        if (isRun && gameObject.transform.localScale.x <= 0)
            isDone = true;
        else if (isRun)
        {
            carObject.GetComponent<Animator>().SetTrigger("isMove");
            StartCoroutine(CarMovingDone(carAnimation.length));
        }
    }

    public IEnumerator ShowButton()
    {
        nextButton.SetActive(false);
        yield return new WaitForSeconds(delayFactor);
        nextButton.SetActive(true);
    }

    public IEnumerator CarMovingDone(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetTrigger("isScaleDown");
    }

    public void OnClickButton()
    {
        isRun = true;
    }
}
