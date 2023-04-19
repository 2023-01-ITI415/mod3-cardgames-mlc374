using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(JsonParseDeck) )]
public class Deck : MonoBehaviour
{
    [Header("Inscribed")]
    public CardSpritesSO    cardSprites;
    public GameObject       prefabCard;
    public GameObject       prefabSprite;
    public bool             startFaceUp = true;

    [Header("Dynamic")]
    public Transform        deckAnchor;
    public List<Card>       cards;

    private JsonParseDeck   jsonDeck;

    static public GameObject SPRITE_PREFAB { get; private set;}

    /*
    // Start is called before the first frame update
    void Start()
    {
        InitDeck();
        Shuffle(ref cards);    
    }
    */

    ///<summary>
    ///the prospector class will call InitDeck to set up the deck and build
    /// all 52 card GameObjects from the jsonDeck and cardSprites information.
    ///</summary>

    public void InitDeck(){
        //create a static reference to spritePrefab for the Card class to use
        SPRITE_PREFAB = prefabSprite;
        //call Init method on the CardSpriteSO instance assigned to cardSprites
        cardSprites.Init();

        //get a reference to the JsonParseDeck component
        jsonDeck = GetComponent<JsonParseDeck>();

        //create an anchor for all the Card GameObjects in the Hierarchy
        if (GameObject.Find("_Deck") == null) {
            GameObject anchorGO = new GameObject( "_Deck");
            deckAnchor = anchorGO.transform;
        }

        MakeCards();
    }

    ///<summary>
    ///Create a GameObject for each card in the deck
    ///</summary>
    void MakeCards() {
        cards = new List<Card>();
        Card c;

        //generate 13 cards for each suit
        string suits = "CDHS";
        for ( int i = 0; i < 4; i++){
            for (int j = 1; j<= 13; j++) {
                c = MakeCard( suits[i], j);
                cards.Add(c);

                //this aligns the cards in nice rows for testing
                c.transform.position =
                    new Vector3((j-7)*3, (i-1.5f)*4,0);
            }
        }
    }

    ///<summary>
    /// Creates a card GameObject based on suit and rank.
    /// NOte that this method assumes it will be passed a valid suit and rank.
    ///</summary>
    ///<param name ="suit"> the suit of the card(e.g., 'C')</param>
    /// <returns></returns>
    Card MakeCard( char suit, int rank){
        GameObject go = Instantiate<GameObject>(prefabCard, deckAnchor);
        Card card = go.GetComponent<Card>();

        card.Init(suit, rank, startFaceUp);
        return card;
    }

    ///<summary>
    /// shuffle a List(card) and return the result to the original list
    /// </summary>
    /// <param name="refCards"> as a ref, this alters on the original list</param>
    static public void Shuffle(ref List<Card> refCards) {
        //create a temporary list to hold the new shuffle order
        List<Card> tCards = new List<Card>();

        int ndx;
        while (refCards.Count > 0){
            ndx = Random.Range(0,refCards.Count);
            tCards.Add (refCards[ndx]);
            refCards.RemoveAt(ndx);
        }
        refCards = tCards;
    }
}
