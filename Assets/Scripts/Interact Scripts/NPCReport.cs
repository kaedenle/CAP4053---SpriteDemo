using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueScriptableObject", menuName = "ScriptableObject/Dialogue")]
public class NPCReport : ScriptableObject
{
    public string conversation_id;
    public Conversation[] conversations;
    public bool loopLast = true;

    public int Length()
    {
        return conversations.Length;
    }

    public int CalculateNextIndex(int current_index)
    {
        if(!loopLast || current_index + 1 < conversations.Length) return current_index + 1;
        return current_index;
    }
}
