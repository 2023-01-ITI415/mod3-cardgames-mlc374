using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///stores info about the layout of the prospector mine
///</summary>
[System.Serializable]
public class JsonLayout {
    public Vector2      multiplier;
    public List<JsonLayoutSlot>     slots;
    public JsonLayoutPile       drawPile, discardPile;
}

/// <summary>
/// stores information for each slot in the layout
/// implements unity's ISerializationCallbackReceiver interface
///</summary>
[System.Serializable]
public class JsonLayoutSlot : ISerializationCallbackReceiver {
    public int          id;
    public int          x;
    public int          y; 
    public bool         faceUp;
    public string       layer;
    public string       hiddenByString;

    [System.NonSerialized]
    public List<int> hiddenBy;

    ///<summary>
    ///pulls data from hiddenBy string and places it into the hiddenBy list
    ///</summary>
    public void OnAfterDeserialize() {
        hiddenBy = new List<int>();
        if (hiddenByString.Length == 0) return;

        string[] bits = hiddenByString.Split(',');
        for (int i=0; i<bits.Length; i++){
            hiddenBy.Add(int.Parse(bits[i]));
        }
    }

    ///<summary>
    /// required by ISerializationCallbackReceiver, but empty in this calss
    ///</summary>
    public void OnBeforeSerialize() {}
}

///<summary>
///stores info for the draw and discard piles
///</summary>
[System.Serializable]
public class JsonLayoutPile{
    public int      x, y;
    public string   layer;
    public float    xStagger;
}

public class JsonParseLayout : MonoBehaviour
{
    public static JsonParseLayout S { get; private set;}

    [Header("Inscribed")]
    public TextAsset    jsonLayoutFile;
    [Header("Dynamic")]
    public JsonLayout layout;

    void Awake(){
        layout = JsonUtility.FromJson<JsonLayout>(jsonLayoutFile.text);
        S = this;
    }
}
