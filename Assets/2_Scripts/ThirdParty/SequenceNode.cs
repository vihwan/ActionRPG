using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SequenceNode : IBTBase
{
    List<IBTBase> _childs;

    public SequenceNode(List<IBTBase> childs)
    {
        _childs = childs;
    }

    public IBTBase.eNodeState Evaluate()
    {
        if (_childs == null || _childs.Count == 0)
            return IBTBase.eNodeState.Fail;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case IBTBase.eNodeState.Update:
                    return IBTBase.eNodeState.Update;
                case IBTBase.eNodeState.Enter:
                    continue;
                case IBTBase.eNodeState.Fail:
                    return IBTBase.eNodeState.Fail;
            }
        }

        return IBTBase.eNodeState.Enter;
    }
}