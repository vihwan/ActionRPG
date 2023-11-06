using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBTBase
{
    public enum eNodeState
    {
        Enter,
        Update,
        Exit,
        Fail,
    }

    public eNodeState Evaluate();
}