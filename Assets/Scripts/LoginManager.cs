using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public FadeController fadeController;
    public PlayerData playerData;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI notifText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupName()
    {
        if (nameInputField.text == "")
            notifText.text = "Harap Masukkan Nama Terlebih Dahulu!";
        else
        {
            playerData.playerName = nameInputField.text.ToUpper();
            notifText.text = $"Selamat Datang, {playerData.playerName}!";
            fadeController.playWhenMove = true;
        }
    }
}
