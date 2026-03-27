using UnityEngine;
public class Ghost : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CountryConfig _country;
    private Vector3 _fixedPosition;
    public CountryConfig Country => _country;
    public Vector3 Position => _fixedPosition;

    public void Initialize(CountryConfig country, float alpha)
    {
        _country = country;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetAlphaColor(alpha);
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
