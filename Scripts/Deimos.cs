using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Reflection;
using Godot;

/// <summary>
/// Responsible to detect user input and execute logic
/// </summary>
public class Deimos : MouseOrKeyboardInputs
{
    // Player stats
    [Category("Deimos' stats")]
    [Export] float speed = 300f;
    [Export] float reducedSpeed = 200f;
    [Export] float distanceToMerge = 150;

    [Category("Deimos' bullets")]
    [Export] float bulletSpeed = 1100f;
    [Export] float fireRate = .2f;
    float fireRateOppositeCoeff = .85f;
    float matchingDirFireRateCoeffe = 1.1f;
    
    // nodepaths
    [Category("NodePath")]
    [Export] NodePath ShootFactoryPath, cursorPath, lifePath, pholiaPath, renderPath;

    // Component
    ShootComponent shootComponent;
    Cursor cursorComponent;
    LifeComponentCollision life;
    AnimatedSprite renderer;
    Pholia pholia;

    // Statemachine
    public Action<float> CurrentState; // arg is delta time
    public event Action onSplit;
    public event Action onResplit;

    // AnimatedSprite anims names
    string A_Whole = "default";
    string A_Splited = "UnGhosted";

    // Dynamic variables
    float shootCooldown;
    private Vector2 playerVel;

    public PlayerState playerState { get; private set; }

    public override void _Ready()
    {
        m_AxisReturnToZero = false;

        shootComponent = GetNode<ShootComponent>(ShootFactoryPath);
        cursorComponent = GetNode<Cursor>(cursorPath);
        life = GetNode<LifeComponentCollision>(lifePath);
        pholia = GetNode<Pholia>(pholiaPath);
        renderer = GetNode<AnimatedSprite>(renderPath);

        base._Ready();

        life.onNoMoreHealth += Death;

        SetModeNormal();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        CurrentState?.Invoke(delta);
    }
    
    private void Move(float delta, float dynamicSpeed)
    {
        Vector2 lInputPlayer = new Vector2(Input.GetAxis(INPUTS.LEFT, INPUTS.RIGHT), Input.GetAxis(INPUTS.UP, INPUTS.DOWN));
        playerVel = lInputPlayer.Normalized() * dynamicSpeed;
        GlobalPosition += playerVel * delta;
    }

    private void SetModeNormal()
    {
        CurrentState = NormalMode;
        playerState = PlayerState.Normal;
    }
    private void NormalMode(float delta)
    {
        renderer.Play(A_Whole);
        cursorComponent?.SetCursorDirection(aimInputAxis);

        Move(delta, speed);
        shootCooldown = Mathf.MoveToward(shootCooldown, 0f, delta);

        if (Input.IsActionPressed(INPUTS.FIRE) && shootCooldown <= 0)
        {
            float dotSign = Mathf.Sign(playerVel.Normalized().Dot(cursorComponent.originToCursorDirection));


            float lBulletSpeed = bulletSpeed;
            Vector2 bulletVel = (cursorComponent.originToCursorDirection.Normalized() * lBulletSpeed);

            if (bulletVel == Vector2.Zero)
                bulletVel = Vector2.Up;
            
            // change hard coded value later
            shootComponent.Shoot(GameManager.bulletContainer, bulletVel, cursorComponent.cursorGlobalPosition);
            if (dotSign > 0)
                shootCooldown += fireRate * matchingDirFireRateCoeffe;
            else if (dotSign < 0) 
                shootCooldown += fireRate * fireRateOppositeCoeff;
            else shootCooldown += fireRate;
        }

        else if (Input.IsActionJustPressed(INPUTS.SPLIT))
        {
            SetModeSplit();
        }
    }

    private async void SetModeSplit()
    {
        playerState = PlayerState.InTransition;
        CurrentState = null;
        renderer.Play(A_Splited);
        pholia.GlobalPosition = GlobalPosition;
        pholia.Visible = true;

        Vector2 finalPosDeimos = GlobalPosition - (aimInputAxis * 100f);
        Vector2 finalPosPholia = GlobalPosition + (aimInputAxis * 100f);
        float animationDuration = .3f;

        SceneTreeTween tween = CreateTween();
        tween.TweenProperty(this, "global_position", finalPosDeimos, animationDuration)
            .SetTrans(Tween.TransitionType.Circ)
            .SetEase(Tween.EaseType.Out);
        tween.Parallel().TweenProperty(pholia, "global_position", finalPosPholia, animationDuration)
            .SetTrans(Tween.TransitionType.Circ)
            .SetEase(Tween.EaseType.Out);

        await ToSignal(tween, SignalNames.TWEEN_FINISHED);


        CurrentState = SplittedMode;
        onSplit?.Invoke();
        cursorComponent.Visible = false;
        playerState = PlayerState.Splitted;
    }
    private void SplittedMode(float delta)
    {
        Move(delta, reducedSpeed);

        if (Input.IsActionPressed(INPUTS.SPLIT) && pholia.AskResplit)
        {
            if (DistanceBetweenDeimosAndPholia() >= distanceToMerge) return;
            
            Resplit();

            return;
        }
    }

    private float DistanceBetweenDeimosAndPholia() => (pholia.GlobalPosition - GlobalPosition).Length();

    private async void Resplit()
    {
        playerState = PlayerState.InTransition;
        CurrentState = null;
        onResplit?.Invoke();

        Vector2 deimosPos = GlobalPosition;
        Vector2 pholiaPos = pholia.GlobalPosition;
        Vector2 middle = new Vector2(Mathf.Lerp(deimosPos.x, pholiaPos.x, 0.5f), Mathf.Lerp(deimosPos.y,pholiaPos.y,0.5f));
        float timeToMerge = .3f;

        SceneTreeTween tween = CreateTween();
        tween.TweenProperty(this, "global_position", middle, timeToMerge)
            .SetTrans(Tween.TransitionType.Circ)
            .SetEase(Tween.EaseType.Out);
        tween.Parallel().TweenProperty(pholia, "global_position", middle, timeToMerge)
            .SetTrans(Tween.TransitionType.Circ)
            .SetEase(Tween.EaseType.Out);

        tween.Play();
        await ToSignal(tween, SignalNames.TWEEN_FINISHED);

        pholia.Visible = false;
        cursorComponent.Visible = true;
        shootCooldown += fireRate;
        SetModeNormal();
    }

    private void Death()
    {
        Visible = false;
        CurrentState = null;
    }

}

public enum PlayerState
{
    Normal,
    Splitted,
    InTransition
}