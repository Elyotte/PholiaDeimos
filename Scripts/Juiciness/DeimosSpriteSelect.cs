using Godot;
using System;
using System.ComponentModel.Design;

public class DeimosSpriteSelect : Sprite
{
    [Export] Texture splittedTexture;
    [Export] NodePath deimospth;

    Texture startingSprite;
    Deimos deimos; 
    public enum TextureMode
    {
        Deimos,
        PholiaAndDeimos
    }

    public override void _Ready()
    {
        startingSprite = Texture;
        CallDeferred(nameof(Start));
    }

    void Start()
    {
        deimos = GetNode<Deimos>(deimospth);
        deimos.onResplitAnimFinished += OnResplit;
        deimos.onSplit += OnSplit;
    }

    void OnResplit()
    {
        Texture = startingSprite;
    }

    void OnSplit()
    {
        Texture = splittedTexture;
    }

    public override void _ExitTree()
    {
        deimos.onResplitAnimFinished -= OnResplit;
        deimos.onSplit -= OnSplit;
        base._ExitTree();
    }
}
