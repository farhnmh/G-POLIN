using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public PlayerData playerData;
    public TextMeshProUGUI helloText;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("ImportantHandler").GetComponent<PlayerData>();
        helloText.text = $"Hai, {playerData.playerName}!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
