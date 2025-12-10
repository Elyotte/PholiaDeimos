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

    public override void _Ready()
    {
        Hide();
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
