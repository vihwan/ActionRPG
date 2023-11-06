using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ActionNode : IBTBase
{
    Func<IBTBase.eNodeState> _onUpdate = null;

    public ActionNode(Func<IBTBase.eNodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public IBTBase.eNodeState Evaluate() => _onUpdate?.Invoke() ?? IBTBase.eNodeState.Fail;
}
