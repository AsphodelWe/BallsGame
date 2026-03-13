using UnityEngine;
public class Ghost
{
    private GameObject _gameObject;
    private SpriteRenderer _spriteRenderer;
    private CountryConfig _country;
    private Vector3 _fixedPosition;
    public CountryConfig Country => _country;
    public Vector3 Position => _fixedPosition;
    public Ghost(CountryConfig country, float alpha)
    {
        _gameObject = Object.Instantiate(country.BallPrefabPrew);
        _spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
        _country = country;
        SetAlphaColor(alpha);
    }

    public void SetPosition(Vector3 position)
    {
        _gameObject.transform.position = position;
    }

    public void Hide()
    {
        _gameObject.SetActive(false);
    }
    
    public void Show()
    {
        _gameObject.SetActive(true);
    }
    
    public void Destroy()
    {
        Object.Destroy(_gameObject);
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
        _fixedPosition = _gameObject.transform.position;
    }
}
