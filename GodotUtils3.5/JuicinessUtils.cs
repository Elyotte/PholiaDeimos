using Godot;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

public static class JuicinessUtils
{
    static Task tick;
    static CancellationTokenSource cancelSource;

    public static SceneTree GetMainTree()
    {
        return Engine.GetMainLoop() as SceneTree;
    }

    public static async Task HitStop(float pDuration, float stopTimeScale = 0f)
    {
        float initialTimeScale = Engine.TimeScale;
        Engine.TimeScale = stopTimeScale;
        await Task.Delay((int)(pDuration * 1000));
        Engine.TimeScale = initialTimeScale;
    }

    public static async Task SmoothedHitStop(float pDuration, float pTransitionTime, float pStopTimeScale = 0f)
    {
        float initialTimeScale = Engine.TimeScale;
        cancelSource = new CancellationTokenSource();

        // Freeze instantané
        Engine.TimeScale = pStopTimeScale;

        // Maintien du freeze
        await Task.Delay((int)(pDuration * 1000));

        tick = ReturnToNormalTimeScale(pTransitionTime, pStopTimeScale, initialTimeScale, cancelSource.Token);
    }

    private static async Task ReturnToNormalTimeScale(float pTransition, float pStopTimeScale, float pTargetScale, CancellationToken tk)
    {
        float initialScale = Engine.TimeScale;
        int elapsedMilliseconds = 0;
        int maxMilliseconds = (int)(pTransition * 1000);

        int delta = 100;

        while (elapsedMilliseconds <= maxMilliseconds)
        {
            elapsedMilliseconds += delta;
            Engine.TimeScale = Mathf.Lerp(initialScale, pTargetScale, (float)elapsedMilliseconds / maxMilliseconds);
            GD.Print(Engine.TimeScale);
            await Task.Delay(delta);
        }

        Engine.TimeScale = pTargetScale;
    }

    /// <summary>
    /// This method provide a way to make a camera shake using parameters and camera reference
    /// The async method is unscaled, meaning that the shake still works when Timescale is 0
    /// </summary>
    /// <param name="pCamera"> Camera reference</param>
    /// <param name="pShakeDuration"> Duration of the shake </param>
    /// <param name="pShakeIntensity"> the intensity of the shake in pixels </param>
    /// <param name="token"> optionnal token to be able to cancel the shake</param>
    /// <returns></returns>
    public static async Task Shake(Camera2D pCamera, float pShakeDuration, float pShakeIntensity, CancellationToken token = default)
    {
        if (pCamera == null) return;

        Vector2 originalOffset = pCamera.Offset;
        int elapsed = 0;
        int shakeDuration = (int)pShakeDuration * 1000;
        int iDelta = 16;
        RandomNumberGenerator rng = new RandomNumberGenerator();

        rng.Randomize();
        try
        {
            while (elapsed < shakeDuration && !token.IsCancellationRequested)
            {
                elapsed += iDelta;

                float fadeOut = 1f - ((float)elapsed / shakeDuration);
                float currentIntensity = pShakeIntensity * fadeOut;

                float offsetX = rng.RandfRange(-1f, 1f) * currentIntensity;
                float offsetY = rng.RandfRange(-1f, 1f) * currentIntensity;

                pCamera.Offset = originalOffset + new Vector2(offsetX, offsetY);

                await Task.Delay(iDelta);
            }
        }
        finally
        {
            pCamera.Offset = originalOffset;
        }


    }

}
