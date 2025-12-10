using Godot;
using System;
using System.Threading.Tasks;

public class ShieldedEnemy : PatternBasedEnemy
{
    Vector2 targetPos;
    float _DeadZone = 50f;
    float elapsedTime = 0f;

    // secondary pattern var
    float timeInSecondPattern = 2f;
    [Export] float spawnRate = 2f;

    // third pattern
    Vector2 anchorPoint;
    RandomNumberGenerator rng;
    float radius = 10f;
    float thirdStateTime = 2f;

    float spawnRandomRadius = 120f;
    float spawnY = 90f;

    [Export] PackedScene enemyFab;

    public override void _Ready()
    {
        base._Ready();
        m_Margin = -50;
        targetPos = new Vector2(screensize.x * .5f, screensize.y * .3f);
    }
    protected override void Pattern(float delta)
    {
        if (IsOutOfBounds(out Vector2 pBounds))
        {
            GlobalPosition += -pBounds.Normalized() * m_MoveSpeed * delta;
        }
        Vector2 distanceToTarget = targetPos - GlobalPosition;
        
        if(distanceToTarget.Length() >= _DeadZone)
        {
            GlobalPosition += distanceToTarget.Normalized() * m_MoveSpeed * delta;
        }
        else
        {
            SetSecondaryPattern();
        }

    }

    protected void SetSecondaryPattern()
    {
        elapsedTime = timeInSecondPattern;
        m_CurrentState = SecondaryPattern;
    }
    protected void SecondaryPattern(float delta)
    {
        HorizontalSine(delta);
        GlobalPosition += Vector2.Down * (0.5f * m_MoveSpeed) * delta;

        elapsedTime -= delta;
        if (elapsedTime <= 0) {
            elapsedTime += spawnRate;
            SetThirdPattern();
        }
    }

    protected void SetThirdPattern()
    {
        elapsedTime = thirdStateTime;
        anchorPoint = GlobalPosition;
        rng = new RandomNumberGenerator();
        rng.Randomize();
        m_CurrentState = ThirdPattern;
    }
    protected void ThirdPattern(float delta)
    {
        rng.Randomize();
        GlobalPosition = anchorPoint + new Vector2(rng.RandfRange(-radius, radius), rng.RandfRange(-radius, radius));
        elapsedTime = Mathf.MoveToward(elapsedTime, 0, delta);
        if (elapsedTime <= 0) {
            SpawnEnemies();
            SetSecondaryPattern();
        }
    }

    protected async void SpawnEnemies()
    {
        PopCorn lPopCorn = enemyFab.Instance<PopCorn>();
        GetTree().CurrentScene.AddChild(lPopCorn);
        lPopCorn.GlobalPosition = GlobalPosition;
        SceneTreeTween spawnAnim = GetTree().CreateTween();
        lPopCorn.SetVoid();

        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        Vector2 spawnTarget = GlobalPosition + 
            new Vector2(rng.RandfRange(-spawnRandomRadius, spawnRandomRadius), -spawnY);
        
        spawnAnim.TweenProperty(lPopCorn, "global_position", spawnTarget, .3f)
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.Out);

        await ToSignal(spawnAnim, SignalNames.TWEEN_FINISHED);

        lPopCorn.SetMove();

    }
}
