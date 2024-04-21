using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleSO : ScriptableObject
{
    [SerializeField] private string moduleName;

    public IModule Module { get; set; }
    
    public string ModuleName => moduleName;
}
