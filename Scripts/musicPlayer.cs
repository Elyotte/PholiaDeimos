using Godot;
using System;

public class musicPlayer : AudioStreamPlayer
{
    public event Action onMusicStoped;

    public override void _Ready()
    {
        CallDeferred(nameof(Start));
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.onGameStart += StartMusic;
            GameManager.onGameStop += StopMusicWithPitchDeceleration;
        }
    }

    public async void StopMusicWithPitchDeceleration()
    {
        float elapsed = 0;
        float lerpDuration = 2f;
        while (elapsed <= lerpDuration) 
        {
            elapsed += GetProcessDeltaTime();
            PitchScale = Mathf.Lerp(1, 0.1f, elapsed / lerpDuration);
            await ToSignal(GetTree(), "idle_frame");
        }
        Stop();
        PitchScale = 1;

        onMusicStoped?.Invoke();
    }

    public void StartMusic()
    {
        Play(0);
    }

    public override void _ExitTree()
    {
        if (GameManager.Instance != null)
        {
            GameManager.onGameStop -= StopMusicWithPitchDeceleration;
            GameManager.onGameStart -= StartMusic;
        }
        base._ExitTree();
    }
}
