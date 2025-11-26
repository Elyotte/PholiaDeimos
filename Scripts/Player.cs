using Godot;
using System;

public class Player : BorderCheck
{
    [Export] float speed = 500f;
    [Export] NodePath collisionDamage;
    Area2D areaDamage;

    public override void _Ready()
    {
        areaDamage = GetNode<Area2D>(collisionDamage);
        base._Ready();

        // Connect events
        areaDamage.Connect(SignalNames.AREA_ENTERED, this, nameof(OnAreaEntered));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        Vector2 lInputPlayer = new Vector2(Input.GetAxis(INPUTS.LEFT,INPUTS.RIGHT),Input.GetAxis(INPUTS.UP, INPUTS.DOWN));

        GlobalPosition += lInputPlayer.Normalized() * speed * delta;


    }

    protected void OnAreaEntered(Area2D pArea) {
        if (pArea is Bullet lBullet)
        {
            GD.Print("I GOT DMAAGED");
        }
    }

    public override void _ExitTree()
    {
        areaDamage.Disconnect(SignalNames.AREA_ENTERED,this,nameof(OnAreaEntered));
        base._ExitTree();
    }

}

