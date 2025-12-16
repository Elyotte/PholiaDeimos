using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public class HandsAnimation : AnimatedSprite
{
    [Export] NodePath deimos, sfxsoundsource;
    Deimos _Deimos;
    string defaultAnimName = "default";

    AudioStreamPlayer sfxHands;
    Color color;

    public override void _Ready()
    {
        Hide();
        color = Modulate;
        if (sfxsoundsource != null) sfxHands = GetNode<AudioStreamPlayer>(sfxsoundsource);
        
        if (deimos != null)
        {
            _Deimos = GetNode<Deimos>(deimos);
            _Deimos.onResplit += PlayAnim;
        }
    }

    void Hide() { Visible = false; }

    void OnFrameChanged()
    {
        if (Frame == 8)
        {
            sfxHands?.Play();
        }
    }

    async void PlayAnim()
    {

        GlobalPosition = _Deimos.GlobalPosition;
        Frame = 0;
        Visible = true;
        Modulate = new Color(color.r,color.g, color.b, .7f);
        SceneTreeTween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate", new Color(color.r, color.g, color.b, 0), 1.2f);

        Play(defaultAnimName);
        Connect(SignalNames.FRAME_CHANGED,this,nameof(OnFrameChanged));

        await ToSignal(this, SignalNames.ANIMATION_FINISHED);

        Stop();
        sfxHands?.Stop();
        Frame = 0;
        Disconnect(SignalNames.FRAME_CHANGED,this,nameof(OnFrameChanged));
        Hide();
    }
}
