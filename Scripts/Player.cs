using Godot;
using System;

public class Player : BorderCheck
{
    [Export] float speed = 500f;
    

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        Vector2 lInputPlayer = new Vector2(Input.GetAxis(INPUTS.LEFT,INPUTS.RIGHT),Input.GetAxis(INPUTS.UP, INPUTS.DOWN));

        GlobalPosition += lInputPlayer.Normalized() * speed * delta;


    }

}

