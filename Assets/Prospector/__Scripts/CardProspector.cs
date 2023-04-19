using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this enum defines the variable type eCardState with four named values
public enum eCardState { drawpile, mine, target, discard}

public class CardProspector : Card
{
    [Header("Dynamic: CardProspector")]
    public eCardState           state = eCardState.drawpile;
    public List<CardProspector> hiddenBy = new List<CardProspector>();
    public int layoutID;
    public JsonLayoutSlot   layoutSlot;

    ///<summary>
    ///informs the Prospector class that this card has been clicked
    ///</summary>
    override public void OnMouseUpAsButton() {
        //base.OnMouseUpAsButton();
        Prospector.CARD_CLICKED(this);
    }

}
