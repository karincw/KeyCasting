using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/AnimParamChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "AnimParamChange", message: "set to [anim]", category: "Events", id: "6ab4ad2316672d09f64a58315e965ad1")]
public sealed partial class AnimParamChange : EventChannel<string> { }

