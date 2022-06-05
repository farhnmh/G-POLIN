using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KesukaanKarakter : MonoBehaviour
{
    [System.Serializable]
    public class FoodDetail
    {
        public string foodName;
        public int favouriteValue;
    };

    [System.Serializable]
    public class CharacterFavourite
    {
        public string name;
        public List<FoodDetail> favouriteFoods;
    };

    public List<CharacterFavourite> characterFavourites;
}
