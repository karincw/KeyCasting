using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomState", story: "[wizard] Random State", category: "Action", id: "aec3f47589947c4ca464974a5dec0900")]
public partial class RandomStateAction : Action
{
    [SerializeReference] public BlackboardVariable<Wizard> Wizard;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Wizard.Value.RandomStateChange();
        return Status.Success;
    }
}

