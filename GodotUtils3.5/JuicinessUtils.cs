using Godot;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

public static class JuicinessUtils
{
    static Task tick;
    static CancellationTokenSource cancelSource;
    private static RandomNumberGenerator rng = new RandomNumberGenerator();

    // Delta seconds for asynchronous Engine Timescale independant animations
    const int iDelta = 16; // number of miliseconds between frame for 60FPS
    const float delta = 0.016f; // 16ms precalculated 

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pDuration"></param>
    /// <param name="pTransitionTime"></param>
    /// <param name="pStopTimeScale"></param>
    /// <returns></returns>
    public static async Task SmoothedHitStop(float pDuration, float pTransitionTime, float pStopTimeScale = 0f)
    {
        float initialTimeScale = Engine.TimeScale;
        cancelSource = new CancellationTokenSource();

        // Freeze
        Engine.TimeScale = pStopTimeScale;
        await Task.Delay((int)(pDuration * 1000));

        /* start an interpolation to the original timescale without awaiting to let
           player or any object that called the hitstop resume processing */
        tick = ReturnToNormalTimeScale(pTransitionTime, pStopTimeScale, initialTimeScale, cancelSource.Token);
    }

    private static async Task ReturnToNormalTimeScale(float pTransition, float pStopTimeScale, float pTargetScale, CancellationToken tk)
    {
        float elapsed = 0f;

        try
        {
            while (elapsed < pTransition && !tk.IsCancellationRequested)
            {
                elapsed += delta;
                float t = Mathf.Clamp(elapsed / pTransition, 0f, 1f);
                Engine.TimeScale = Mathf.Lerp(pStopTimeScale, pTargetScale, t);

                await Task.Delay(iDelta);
            }
        }
        finally
        {
            Engine.TimeScale = pTargetScale;
        }
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
        float elapsed = 0f;

        try
        {
            while (elapsed < pShakeDuration && !token.IsCancellationRequested)
            {
                elapsed += delta;
                float fadeOut = 1f - (elapsed / pShakeDuration);
                float currentIntensity = pShakeIntensity * fadeOut;

                pCamera.Offset = originalOffset + new Vector2(
                    rng.RandfRange(-currentIntensity, currentIntensity),
                    rng.RandfRange(-currentIntensity, currentIntensity)
                );

                await Task.Delay(iDelta);
            }
        }
        finally
        {
            if (pCamera != null) pCamera.Offset = originalOffset;
        }
    }

}
