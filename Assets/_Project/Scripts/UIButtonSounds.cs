using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIButtonSounds : MonoBehaviour
{
    [SerializeField] private AudioClip _defaultClickSound;
    [Range(0f, 1f)][SerializeField] private float _volume = 1f;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        root.RegisterCallback<ClickEvent>(evt =>
        {
            if (evt.target is Button)
            {
                AudioSource.PlayClipAtPoint(_defaultClickSound, Camera.main.transform.position, _volume);
            }
        });
    }
}
