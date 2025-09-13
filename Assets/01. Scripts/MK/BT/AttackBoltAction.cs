using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackBolt", story: "[wizard] attack bolt", category: "Action", id: "25a4d3c42bc267f0be3872d5ca2eed3a")]
public partial class AttackBoltAction : Action
{
    [SerializeReference] public BlackboardVariable<Wizard> Wizard;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Wizard.Value.Attack();
        return Status.Success;
    }
}

