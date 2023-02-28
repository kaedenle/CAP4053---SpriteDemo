using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScriptable
{
    void ScriptHandler(bool flag);
    void EnableByID(int ID);
    void DisableByID(int ID);
}
