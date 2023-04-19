using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Dynamic")]
    public char             suit; //suit of the card (c,d,h, or s)
    public int              rank; //rank of the card (1-13)
    public Color            color = Color.black; //color to tint pips
    public string           colS = "Black"; // or "red". name of the color
    public GameObject       back; //the gameobject of the back of the card
    public JsonCard         def; //the card layout as defind in the JSON_Deck.json

    //this list holds all of the decorator gameobjects
    public List<GameObject> decoGOs = new List<GameObject>();
    //this list holds all of the pip gameobjects
    public List<GameObject> pipGOs = new List<GameObject>();

    ///<summary>
    ///creates this card's visuals based on suit and rank
    /// note that this mehtod assumes it will be passed a valid suit and rank
    ///</summary>
    ///<param name ="eSuit"> the suit of the card(e.g., 'C')</param>
    /// <param name = "eRank"> the rank from 1 to 13</param>
    /// <returns></returns>
    public void Init(char eSuit, int eRank, bool startFaceUp=true){
        //assign basic values to the card
        gameObject.name = name = eSuit.ToString() + eRank;
        suit = eSuit;
        rank = eRank;
        //if this is a diamond or heart, change the default Black color to Red
        if (suit == 'D' || suit == 'H') {
            colS = "Red";
            color = Color.red;
        }

        def = JsonParseDeck.GET_CARD_DEF(rank);

        //build the card from sprites
        AddDecorators();
        AddPips();
        AddFace();
        AddBack();
        faceUp = startFaceUp;
    }

    ///<summary>
    ///shortcut for setting transform.localPosition
    ///</summary>
    ///<param name="v"></param>
    public virtual void SetLocalPos(Vector3 v) {
        transform.localPosition = v;
    }

    //these private variables that will be resused several times
    private Sprite      _tSprite = null;
    private GameObject  _tGO = null;
    private SpriteRenderer _tSRend = null;
    //an euler rotation of 180 around the z-axis will flip sprites upside down
    private Quaternion  _flipRot = Quaternion.Euler(0,0,180);

    ///<summary>
    /// adds the decorators to the top-left and bottom-right of each card.
    /// decorators are the suit and rank in the corners of each card
    ///</summary>
    private void AddDecorators() {
        //add decorators
        foreach (JsonPip pip in JsonParseDeck.DECORATORS) {
            if (pip.type == "suit") {
                //Instantiate a Sprite GameObject
                _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB,transform);
                //get the spriterenderer component
                _tSRend.sprite = CardSpritesSO.SUITS[suit];
            } else {
                _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB,transform);
                _tSRend = _tGO.GetComponent<SpriteRenderer>();
                //get the rank sprite from the card spritesSO.ank static field
                _tSRend.sprite = CardSpritesSO.RANKS[rank];
                //set the color of the rank to match the suit
                _tSRend.color = color;
            }

            //make the decorator sprites render above the card
            _tSRend.sortingOrder = 1;
            //set the local position based on the location from DeckXML
            _tGO.transform.localPosition = pip.loc;
            //flip the decorator if needed
            if (pip.flip) _tGO.transform.rotation = _flipRot;
            //set the scale to keep decorators from being too big
            if (pip.scale !=1) {
                _tGO.transform.localScale = Vector3.one * pip.scale;
            }
            //name this gameobject so its easy to find in the hierarchy
            _tGO.name = pip.type;
            //add this decorator gameobject to the list card.decoGOs
            decoGOs.Add(_tGO);
        }
    }

    ///<summary>
    /// adds pips to the front of all cards from A to 10
    ///</summary>
    private void AddPips() {
        int pipNum = 0;
        //for each of the pips in the definition...
        foreach (JsonPip pip in def.pips) {
            //instantiate a gameobject from the Deck.SPRITE_PREFAB static field
            _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
            _tGO.transform.localPosition = pip.loc;
            if (pip.flip) _tGO.transform.rotation = _flipRot;
            if (pip.scale != 1){
                _tGO.transform.localScale = Vector3.one * pip.scale;
            }
            _tGO.name = "pip_"+pipNum++;
            _tSRend = _tGO.GetComponent<SpriteRenderer>();
            _tSRend.sprite = CardSpritesSO.SUITS[suit];
            _tSRend.sortingOrder = 1;
            pipGOs.Add(_tGO);
        }
    }

    ///<summary>
    ///Adds the face sprite for card ranks 11-13
    /// </summary>
    private void AddFace() {
        if (def.face == "")
            return;//no need to run if not a face card
        //find a face sprite in CardSPritesSO with the right name
        string faceName = def.face + suit;
        _tSprite = CardSpritesSO.GET_FACE( faceName);
        if ( _tSprite == null) {
            Debug.LogError( "Face srite " + faceName + " not found.");
            return;
        }

        _tGO = Instantiate<GameObject>( Deck.SPRITE_PREFAB, transform);
        _tSRend = _tGO.GetComponent<SpriteRenderer>();
        _tSRend.sprite = _tSprite;
        _tSRend.sortingOrder = 1;
        _tGO.transform.localPosition = Vector3.zero;
        _tGO.name = faceName;
    }

    ///<summary>
    /// property to show and hide the back of the card
    /// <summary>
    public bool faceUp{
        get { return (!back.activeSelf);}
        set { back.SetActive(!value);}
    }

    ///<summary>
    /// adds a back to the card so that renders on top of everything else
    ///</summary>
    private void AddBack() {
        _tGO = Instantiate<GameObject>( Deck.SPRITE_PREFAB, transform);
        _tSRend = _tGO.GetComponent<SpriteRenderer>();
        _tSRend.sprite = CardSpritesSO.BACK;
        _tGO.transform.localPosition = Vector3.zero;
        //2 is a higher sortingOrder than anything else
        _tSRend.sortingOrder = 2;
        _tGO.name = "back";
        back = _tGO;
    }

    private SpriteRenderer[] spriteRenderers;

    ///<summary>
    /// gather all spriterenderers on this and its children into an array
    ///</summary>
    void PopulateSpriteRenderers(){
        if (spriteRenderers != null) return;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetSpriteSortingLayer (string layerName) {
        PopulateSpriteRenderers();

        foreach (SpriteRenderer srend in spriteRenderers) {
            srend.sortingLayerName = layerName;
        }
    }

    ///<summary>
    ///sets the sortingOrder of the sprites on this card
    /// allows multiple cards to be used in the same sorting layer and still overlap properly
    /// is used by both draw and discard piles
    ///</summary>
    ///<param name=sOrd">The sortingOrder for the face of the card"</param>
    public void SetSortingOrder(int sOrd){
        PopulateSpriteRenderers();

        foreach (SpriteRenderer srend in spriteRenderers){
            if (srend.gameObject == this.gameObject){
                srend.sortingOrder = sOrd;
            } else if (srend.gameObject.name == "back") {
                srend.sortingOrder = sOrd+2;
            } else {
                srend.sortingOrder = sOrd + 1;
            }
        }
    }
    virtual public void OnMouseUpAsButton() {
        print(name);
    }

    ///<summary>
    ///return true if cards are adjacent in rank
    ///if warp is true, ace and king are adjacent
    ///</summary>
    /// <param name="otherCard">The card to compare to</param>
    ///<param name="wrap">if true (default) ace and king wrap</param>
    /// <returns> true if the cards are adjacent</returns>
    public bool AdjacentTo(Card otherCard, bool wrap=true) {
        if (!faceUp || !otherCard.faceUp) return (false);
        
        if(Mathf.Abs(rank - otherCard.rank) ==1) return (true);

        if (wrap) {
            if (rank ==1 && otherCard.rank == 13) return (true);
            if (rank ==13 && otherCard.rank == 1) return (true);
        }

        return(false);
    }
}
