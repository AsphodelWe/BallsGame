using UnityEngine;

public interface IMasterFactory
{
    Master CreateMaster(AttackerConfig config, SideConfig side, Transform slot);
}
