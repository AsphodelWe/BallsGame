using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        return _listBall.FirstOrDefault(b => b.GetSide != mySide);
    }

    public IReadOnlyList<BallPresent> GetAllBalls() => _listBall;
}
