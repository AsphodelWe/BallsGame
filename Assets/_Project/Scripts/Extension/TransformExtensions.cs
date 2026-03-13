using UnityEngine;

public static class TransformExtensions
{
    public static void ResetLocal(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void ResetLocal(this Transform transform, float targetWorldScale)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.SetWorldScaleFloat(targetWorldScale);
    }

    public static float GetWorldScale(this Transform transform)
    {
        float scale = transform.localScale.x;
        var current = transform.parent;

        while (current != null)
        {
            scale *= current.localScale.x;
            current = current.parent;
        }

        return scale;
    }

    public static void SetWorldScaleFloat(this Transform transform, float targetWorldScale)
    {
        float parentScale = 1f;
        var current = transform.parent;

        while (current != null)
        {
            parentScale *= current.localScale.x;
            current = current.parent;
        }

        transform.localScale = Vector3.one * (targetWorldScale / parentScale);
    }

public static void ApplyCurrentScaleToWorld(this Transform transform)
{
    Vector3 currentLocalScale = transform.localScale;
    
    float parentScaleX = 1f;
    float parentScaleY = 1f;
    float parentScaleZ = 1f;
    var current = transform.parent;
    
    while (current != null)
    {
        parentScaleX *= current.localScale.x;
        parentScaleY *= current.localScale.y;
        parentScaleZ *= current.localScale.z;
        current = current.parent;
    }
    
    transform.localScale = new Vector3(
        currentLocalScale.x / parentScaleX,
        currentLocalScale.y / parentScaleY,
        currentLocalScale.z / parentScaleZ
    );
}
}
