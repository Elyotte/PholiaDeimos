using System;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public class CameraManager : Camera2D
{
    [Export]
    public Curve ShakeIntensityCurve;
    public static CameraManager Instance { get; private set; }
    Vector2 startOffset;
    RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
       Instance = this;
        if (ShakeIntensityCurve == null)
        {
            ShakeIntensityCurve = new Curve();
            ShakeIntensityCurve.AddPoint(new Vector2(0f, 1f));
            ShakeIntensityCurve.AddPoint(new Vector2(0.3f, 0.8f));
            ShakeIntensityCurve.AddPoint(new Vector2(1f, 0f));
        }
        startOffset = Offset;
        CallDeferred(nameof(Subscribe));
        rng.Randomize();
    }

    void Subscribe()
    {
        if (GameManager.Instance == null) return;

        GameManager.deimos.onResplitAnimFinished += ResplitShake;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }
    public async Task Shake( float pShakeDuration, float pShakeIntensity, CancellationToken token = default)
    {
        if (!Current) return;

        Vector2 originalOffset = Offset;
        int elapsed = 0;
        int shakeDuration = (int)pShakeDuration * 1000;
        int iDelta = 16;

        rng.Randomize();
        while (elapsed < shakeDuration && !token.IsCancellationRequested)
        {
            elapsed += iDelta;

            float fadeOut = 1f - ((float)elapsed / shakeDuration);
            float currentIntensity = pShakeIntensity * fadeOut;

            float offsetX = rng.RandfRange(-1f,1f) * currentIntensity;
            float offsetY = rng.RandfRange(-1f, 1f) * currentIntensity;

            Offset = originalOffset + new Vector2(offsetX, offsetY);

            await Task.Delay(iDelta);
            GD.Print(elapsed);
        }

        Offset = originalOffset;
    }

    async void ResplitShake()
    {
        await Shake(1f, 50f);
    }
}
