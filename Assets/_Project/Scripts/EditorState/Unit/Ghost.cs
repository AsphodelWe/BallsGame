using UnityEngine;
public class Ghost : MonoBehaviour
{
    [SerializeField] private float _alpha = 0.5f;
    private SpriteRenderer _spriteRenderer;
    private CountryConfig _country;
    private Vector3 _fixedPosition;
    public CountryConfig Country => _country;
    public Vector3 Position => _fixedPosition;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetAlphaColor(_alpha);
    }
    public void Initialize(CountryConfig country)
    {
        _country = country;
    }

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void SetAlphaColor(float alpha)
    {
        if (_spriteRenderer == null) return;

        var color = _spriteRenderer.color;
        color.a = alpha;
        _spriteRenderer.color = color;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void Place()
    {
        _fixedPosition = gameObject.transform.position;
    }
}
