using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueScriptableObject", menuName = "ScriptableObject/Dialogue")]
public class NPCReport : ScriptableObject
{
    public Conversation[] conversations;
    public string conversation_id;

    public int Length()
    {
        return conversations.Length;
    }
}
