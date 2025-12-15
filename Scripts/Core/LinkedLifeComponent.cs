using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class LinkedLifeComponent : LifeComponentCollision
{
    [Export] NodePath lifeComponentToApplyDamagePath;
    LifeComponentCollision LifeComponentToApplyDamage;

    List<LifeComponentCollision> lifeComponents = new List<LifeComponentCollision>();
    Task taskStay;
    CancellationTokenSource cancellationTokenSource;

    [Export] float dealDamageEverySecondsDelay = .3f;
    [Export] int dealDamageAmount = 3;

    public event Action<LifeComponentCollision[]> onDamageDealt;

    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(Init));
        Connect(SignalNames.AREA_EXITED, this, nameof(OnAreaExited));
    }

    protected override void OnAreaEntered(Area2D area)
    {
        if (area is LifeComponentCollision lLife && !(area.GetParent() is Deimos))
        {
            Add(lLife);
        }
        base.OnAreaEntered(area);

    }

    void Init()
    {
        LifeComponentToApplyDamage = GetNode<LifeComponentCollision>(lifeComponentToApplyDamagePath);
    }

    protected virtual void OnAreaExited(Area2D area)
    {
        if (!(area is LifeComponentCollision lLife))
            return;
        if (lifeComponents.Contains(lLife))
        {
            Remove(lLife);
        }
    }

    void Add(LifeComponentCollision lLife)
    {
        lifeComponents.Add(lLife);
        CheckStayCount();
    }

    void Remove(LifeComponentCollision lLife)
    {
        lifeComponents.Remove(lLife);
        CheckStayCount();
    }

    void CheckStayCount()
    {
        if (lifeComponents.Count > 0)
        {
            if (taskStay == null || taskStay.IsCompleted)
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
                taskStay = DamageEnemiesInStayList(cancellationTokenSource.Token);
            }
        }
        else
        {
            cancellationTokenSource?.Cancel();
        }
    }
    private async Task DamageEnemiesInStayList(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                SceneTreeTimer timer = GetTree().CreateTimer(dealDamageEverySecondsDelay);
                await ToSignal(timer, SignalNames.TIMEOUT);

                int length = lifeComponents.Count;
                for (int i = length - 1; i >= 0; i--)
                {
                    lifeComponents[i].Damage(dealDamageAmount);
                    if (lifeComponents[i].currentLife <= 0)
                        lifeComponents.RemoveAt(i);
                }

                // Send an event with all the infos necessary to applies multiples
                // animations
                onDamageDealt?.Invoke(lifeComponents.ToArray());
            }
        }
        catch (OperationCanceledException)
        {
            GD.Print("Task cancelled");
        }

    }

    public override void Heal(int pHealAmount)
    {
        if (LifeComponentToApplyDamage == null) return;
        LifeComponentToApplyDamage?.Heal(pHealAmount);
    }
    public override void Damage(int pAmount)
    {
        if (LifeComponentToApplyDamage == null) return;
        LifeComponentToApplyDamage.Damage(pAmount);
    }

    public override void _ExitTree()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        base._ExitTree();
    }
}
