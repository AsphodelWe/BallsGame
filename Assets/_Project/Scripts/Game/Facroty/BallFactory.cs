using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BallFactory
{
    [Inject] private BallRegistry _ballRegistry;
    [Inject] private Container _container;
    public BallPresent CreateBall(CountryConfig country, Vector3 position)
    {
        GameObject ball = Object.Instantiate(country.BallPrefabBattle, position, quaternion.identity);

        GameObjectInjector.InjectRecursive(ball, _container);

        if (ball.TryGetComponent<BallPresent>(out var presenter))
        {
            var data = new BallData(country);
            presenter.Inizialize(data);
            _ballRegistry.Register(presenter);
        }
        else
        {
            Debug.LogError($"BallPresenter missing on {country.BallPrefabBattle.name}!");
            return null;
        }

        return presenter;
    }

    public void DestroyAll()
    {
        var balls = _ballRegistry.GetAllBalls();

        foreach (var ball in balls)
        {
            if (ball != null && ball.gameObject != null)
            {
                Object.Destroy(ball.gameObject);
            }
        }

        _ballRegistry.Clear();
    }
}
