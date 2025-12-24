using System;
using System.Collections.Generic;
using Godot;

public class FXPool : Node
{
    [Export] PackedScene fxScene;
    [Export] int poolSize = 20;

    Queue<CPUParticles2D> pool = new Queue<CPUParticles2D>();

    public override void _Ready()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var fx = fxScene.Instance<CPUParticles2D>();
            fx.Emitting = false;
            AddChild(fx);
            pool.Enqueue(fx);
        }
    }

    CPUParticles2D CreateNewFX()
    {
        CPUParticles2D fx = fxScene.Instance() as CPUParticles2D;
        return fx;
    }

    public void PlayFx(Vector2 position, DamageInfo info)
    {
        CPUParticles2D fx;
        if (pool.Count == 0)
        {
            fx = CreateNewFX();
        }
        else fx = pool.Dequeue();

        int initialAmount = fx.Amount;

        fx.GlobalPosition = position;

        fx.Amount = Mathf.Clamp((1 + info.amount) * fx.Amount, 8, 200);
        fx.Direction = info.bulletVelocity.Normalized();
        fx.Spread = info.bulletVelocity == Vector2.Zero ? 180 : fx.Spread;

        fx.Emitting = false;
        fx.CallDeferred("set_emitting", true);

        ReturnLater(fx, fx.Lifetime, initialAmount);
    }

    async void ReturnLater(CPUParticles2D fx, float time, int initialAmount)
    {
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        fx.Emitting = false;
        fx.Amount = initialAmount;
        pool.Enqueue(fx);
    }
}
