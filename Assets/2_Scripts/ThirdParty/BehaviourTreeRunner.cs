using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner
{
    IBTBase _rootNode;
    public BehaviorTreeRunner(IBTBase rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }
}