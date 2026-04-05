using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class BallRegistry
{
    private List<BallPresent> _listBall = new();
    private Dictionary<SideConfig, List<BallPresent>> _ballBySide = new();
    public Action<BallPresent> OnEnemyAdded;
    public Action<BallPresent> OnEnemyRemoved;

    public void Register(BallPresent ball)
    {
        _listBall.Add(ball);

        var side = ball.GetSide;

        if (!_ballBySide.ContainsKey(side))
            _ballBySide[side] = new List<BallPresent>();

        _ballBySide[ball.GetSide].Add(ball);

        OnEnemyAdded?.Invoke(ball);
    }

    public void UnRegister(BallPresent ball)
    {
        _listBall.Remove(ball);

        var side = ball.GetSide;

        if (_ballBySide.ContainsKey(side))
            _ballBySide[side].Remove(ball);

        OnEnemyRemoved?.Invoke(ball);
    }

    public BallPresent FindAnyEnemy(SideConfig mySide)
    {
        var enemies = _listBall.Where(b => b.GetSide != mySide).ToList();
        if (enemies.Count == 0) return null;
        return enemies[UnityEngine.Random.Range(0, enemies.Count)];
    }

    public IReadOnlyList<BallPresent> GetAllBalls() => _listBall;

    public void Clear()
    {
        _listBall.Clear();
        _ballBySide.Clear();
    }
}
