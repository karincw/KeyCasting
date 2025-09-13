using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomMove", story: "[wizard] randomMove [TargetVec]", category: "Action", id: "fb6e7373825abcb8f7100afc29049bf6")]
public partial class RandomMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<Wizard> Wizard;
    [SerializeReference] public BlackboardVariable<Vector3> TargetVec;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        TargetVec.Value = Wizard.Value.MoveTarget();
        return Status.Success;
    }
}

