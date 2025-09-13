using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StateChange", message: "change to [state]", category: "Events", id: "a9a4cea3a4bf32229015198c1f8b70ef")]
public sealed partial class WizardStateChange : EventChannel<WizardState>, ICloneable
{
    public object Clone()
    {
        return Instantiate(this);
    }
}

