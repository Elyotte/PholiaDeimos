using Godot;
using System;

public class Collectible : Area2D
{
    [Export] public int HealAmount = 2;
    float spawnOffset = 20f;

    public override void _Ready()
    {
        Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        AnimateSpawn();
    }

    public void AnimateSpawn()
    {
        SceneTreeTween spawnAnim = GetTree().CreateTween();

        spawnAnim.TweenProperty(this, "global_position", GlobalPosition + new Vector2(0,-spawnOffset), .2f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Circ);

        spawnAnim.TweenProperty(this, "global_position", GlobalPosition, .3f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Elastic);


    }

    void OnAreaEntered(Area2D area)
    {
        if (area is LifeComponentCollision lLife)
        { 
            Disapear();
        }
    }

    void Disapear()
    {
        QueueFree();
    }

    public override void _ExitTree()
    {
        Disconnect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
        base._ExitTree();
    }

}
