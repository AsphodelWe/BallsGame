using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetRegistry", menuName = "Scriptable Objects/TargetRegistry")]
public class TargetRegistry : ScriptableObject
{
    public List<TargetStrategyConfig> AllTargets;

    public NoTargetConfig NoTarget => AllTargets.OfType<NoTargetConfig>().FirstOrDefault();
    public TrackTargetConfig TrackTarget => AllTargets.OfType<TrackTargetConfig>().FirstOrDefault();
    public TrackTargetWithMarkConfig TrackMarkTarget => AllTargets.OfType<TrackTargetWithMarkConfig>().FirstOrDefault();
}
