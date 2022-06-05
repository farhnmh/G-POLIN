using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenentukanKesukaan : MonoBehaviour
{
    public string characterName;
    public string foodName;
    public int favouriteValue;
    public List<GameObject> symbolFavourite;

    void Awake()
    {
        characterName = transform.parent.gameObject.name;
        foodName = name;
    }

    void Update()
    {
        for (int i = 0; i < symbolFavourite.Count; i++)
        {
            if (i == favouriteValue)
                symbolFavourite[i].SetActive(true);
            else
                symbolFavourite[i].SetActive(false);
        }    
    }

    public void ChangeValue()
    {
        if (favouriteValue < 2)
            favouriteValue++;
        else if (favouriteValue == 2)
            favouriteValue = 0;
    }
}
