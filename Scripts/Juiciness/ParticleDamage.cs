using Godot;
using System;

public class ParticleDamage : CPUParticles2D
{

    int amountFactor;
    float initialVel;
    float initialSpread;

    bool elapsed = false;

    float time;

    public override void _Ready()
    {
        initialVel = InitialVelocity;
        initialSpread = Spread;
        amountFactor = Amount;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (!elapsed) return;

        time += delta;
        if (time > Lifetime + .5f)
        {
            Emitting = false;
        }

    }

    public async void ReactToDamage(DamageInfo pDamageInfo)
    {
        if (Emitting) 
        Emitting = false;

        Amount = Mathf.Clamp(
            (1 + pDamageInfo.amount) * amountFactor,
            5,
            200);

        Direction = pDamageInfo.bulletVelocity.Normalized();
        Spread = pDamageInfo.bulletVelocity == Vector2.Zero ? 180 : initialSpread;

        CallDeferred("set_emitting", true);

        elapsed = true;
    }

}
