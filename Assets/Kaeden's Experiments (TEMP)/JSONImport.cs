using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONImport : MonoBehaviour
{
    public TextAsset moveContainer;
    [System.Serializable]
    public class Employee
    {
        //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
        public string firstName;
        public string lastName;
    }
    [System.Serializable]
    public class Employees
    {
        //employees is case sensitive and must match the string "employees" in the JSON.
        public Employee[] employees;
    }
    public Employees empl = new Employees();

    void Start()
    {
        empl = JsonUtility.FromJson<Employees>(moveContainer.text);
    }
}
