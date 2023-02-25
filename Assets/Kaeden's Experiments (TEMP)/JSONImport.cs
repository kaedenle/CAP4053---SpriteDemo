using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONImport : MonoBehaviour
{
    public TextAsset moveContainer;
    [System.Serializable]
    public class MoveList
    {
        //employees is case sensitive and must match the string "employees" in the JSON.
        public Attack[] framedata;
        public int damage;
        public int knockback;
        public int hitstop;
        //move info
        public string hitsTag;      //can make this into an array
        public string[] cancelBy;
    }
    public MoveList myMoveList = new MoveList();
    // Start is called before the first frame update

    void Start()
    {
        myMoveList = JsonUtility.FromJson<MoveList>(moveContainer.text);
    }
}
