using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Explosion : Bullet
{
    [Export] float explosionStrength = 60f;
    [Export] NodePath particlePath;
    CPUParticles2D particle;
    SceneTreeTimer timer;
    float elapsed = 0f;

    [Export] List<NodePath> secondaryParticlesPath = new List<NodePath>();

    public override void _Ready()
    {
        particle = GetNode<CPUParticles2D>(particlePath);
        base._Ready();
        particle.Emitting = true;

        int length = secondaryParticlesPath.Count;
        for (int i = 0; i <length; i++)
        {
            GetNode<CPUParticles2D>(secondaryParticlesPath[i]).Emitting = true;

        }

        timer = GetTree().CreateTimer(particle.Lifetime * .5f);
        timer.Connect(SignalNames.TIMEOUT, this, nameof(Clear));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        elapsed += delta;

        if (elapsed > particle.Lifetime)
        {
            QueueFree();
        }
    }

    void Clear()
    {
        timer.Disconnect(SignalNames.TIMEOUT, this, nameof(Clear));
        Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
    }

    override protected void OnAreaEntered(Area2D area)
    {
        base.OnAreaEntered(area);
        if (area is LifeComponentCollision lLife)
        {
            if (area.GetParent() is Enemy lEnemy)
            {
                Vector2 direction = lEnemy.GlobalPosition - GlobalPosition;
                lEnemy.AddForce(direction.Normalized() * explosionStrength);
                lLife.Damage(new DamageInfo(damage, direction.Normalized()));
            }
        }
    }

    protected override void DestroyBullet()
    {
        
    }

}
