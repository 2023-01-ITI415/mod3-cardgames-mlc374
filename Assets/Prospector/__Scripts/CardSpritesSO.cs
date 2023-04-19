using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(   fileName = "CardSprites",
                    menuName = "ScriptableObjects/CardSpritesSO")]

public class CardSpritesSO : ScriptableObject 
{
    [Header("Card Stock")]
    public Sprite cardBack;
    public Sprite cardBackGold;
    public Sprite cardFront;
    public Sprite cardFrontGold;

    [Header("Suits")]
    public Sprite suitClub;
    public Sprite suitDiamond;
    public Sprite suitHeart;
    public Sprite suitSpade;

    [Header("Pip Sprites")]
    public Sprite[] faceSprites;
    public Sprite[] rankSprites;

    private static CardSpritesSO S;
    public static Dictionary<char, Sprite> SUITS {get; private set;}

    public void Init(){
        INIT_STATICS(this);
    }

    ///<summary>
    /// initializes the static elements of CardSpriteSO
    ///</summary>
    ///<pararm name = "cSSO">CardSpriteSO to be assigned to the Singleton S</param>

    static void INIT_STATICS(CardSpritesSO cSSO) {
        if (S !=null){
            Debug.LogError("CardSprtesSO.S can't be set a 2nd time!");
            return;
        }
        S = cSSO; //initialize the singleton each time the game starts

        //initialize the _SUITS dictionary
        SUITS = new Dictionary<char, Sprite>(){
            { 'C', S.suitClub},
            { 'D', S.suitDiamond},
            { 'H', S.suitHeart},
            { 'S', S.suitSpade}
        };
    }
    public static Sprite[] RANKS {
        get { return S.rankSprites;}
    }

    /// <summary>
    /// Searches S.faceSprites for the one with the right name
    /// </summary>
    /// <param name = "name"> the name to search for</param>
    /// <returns> a face sprite </returns>

    public static Sprite GET_FACE(string name) {
        foreach (Sprite spr in S.faceSprites){
            if (spr.name == name) return spr;
        }
        return null;
    }

    /// <summary>
    /// this public static property makes S.cardBack accessible to other classes
    /// </summary>

    public static Sprite BACK {
        get {return S.cardBack;}
    }

    ///<summary>
    /// call this to reset the singleton S to null at the end of a game
    ///</summary>
    public static void RESET() {
        S = null;
    }
}
