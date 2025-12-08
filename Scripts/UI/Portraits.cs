using Godot;
using System;

public class Portraits : Control
{
    // Nodepaths and components
    [Export] NodePath deimosPortrait = "DeimosPortrait", pholiaPortrait = "PholiaPortrait", playerPath;
    TextureRect deimos, pholia;
    Deimos player;

    // Variables
    float elapsedTime = 0;
    float blinkSpeed = 2f;
    float blinkCurve = .4f;

    // States and events
    Action<float> state;

    public override void _Ready()
    {
        deimos = GetNode<TextureRect>(deimosPortrait);
        pholia = GetNode<TextureRect>(pholiaPortrait);
        player = GetNode<Deimos>(playerPath);

        player.onSplit += Activate;
        player.onResplit += Deactivate;

        Deactivate();
    }

    public override void _Process(float delta)
    {
        state?.Invoke(delta);
    }

    void Deactivate()
    {
        Visible = false;
        state = null;
    }

    void Activate()
    {
        pholia.Visible = false;
        deimos.Visible = false;
        elapsedTime = 0f;
        Visible = true;
        state = ShowPortraitOnInputs;
    }

    void ShowPortraitOnInputs(float delta)
    {
        elapsedTime += delta;
        float raw = (Mathf.Sin(elapsedTime * Mathf.Pi * blinkSpeed) + 1f) / 2f;
        float alpha = Mathf.Pow(raw, blinkCurve);

        pholia.Modulate = new Color(1f, 1f, 1f, alpha);
        deimos.Modulate = new Color(1f, 1f, 1f, alpha);

        // show pholia & deimos
        pholia.Visible = Input.IsActionPressed(INPUTS.FIRE);
        deimos.Visible = Input.IsActionPressed(INPUTS.SPLIT);
    }

    public override void _ExitTree()
    {
        player.onSplit -= Activate;
        player.onResplit -= Deactivate;
        base._ExitTree();
    }

}
