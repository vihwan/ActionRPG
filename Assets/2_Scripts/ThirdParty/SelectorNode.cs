using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SelectorNode : IBTBase
{
    List<IBTBase> _childs;

    public SelectorNode(List<IBTBase> childs)
    {
        _childs = childs;
    }

    public IBTBase.eNodeState Evaluate()
    {
        if (_childs == null)
            return IBTBase.eNodeState.Fail;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case IBTBase.eNodeState.Update:
                    return IBTBase.eNodeState.Update;
                case IBTBase.eNodeState.Enter:
                    return IBTBase.eNodeState.Enter;
            }
        }

        return IBTBase.eNodeState.Fail;
    }
}