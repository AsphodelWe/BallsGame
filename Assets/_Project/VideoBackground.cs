using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class VideoBackground : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private string _imageElementName = "BackgroundImage";

    private void Awake()
    {
        _videoPlayer.Play();
        _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        _videoPlayer.targetTexture = _renderTexture;
        _videoPlayer.isLooping = true;
        _videoPlayer.Play();
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var background = root.Q<Image>(_imageElementName);

        Debug.Log(background);
        if (background != null)
            background.image = _renderTexture;
    }
}
