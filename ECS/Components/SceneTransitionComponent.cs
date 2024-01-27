using System;

public class SceneTransitionComponent
{
    public enum TransitionState
    {
        None,
        TransitionIn,
        TransitionOut
    }

    public TransitionState CurrentTransitionState { get; set; }
    public float TransitionAlpha { get; private set; }
    public Texture2D FadeTexture { get; }

    private const float transitionSpeed = 0.1f;

    public SceneTransitionComponent(Texture2D fadeTexture)
    {
        FadeTexture = fadeTexture ?? throw new ArgumentNullException(nameof(fadeTexture));
        CurrentTransitionState = TransitionState.None;
        TransitionAlpha = 0f;
    }

    public void TransitionIn()
    {
        TransitionAlpha = MathHelper.Clamp(TransitionAlpha + transitionSpeed * (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds, 0f, 1f);
        if (TransitionAlpha >= 1f)
        {
            TransitionAlpha = 1f;
            CurrentTransitionState = TransitionState.None;
        }
    }

    public void TransitionOut()
    {
        TransitionAlpha = MathHelper.Clamp(TransitionAlpha - transitionSpeed * (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds, 0f, 1f);
        if (TransitionAlpha <= 0f)
        {
            TransitionAlpha = 0f;
            CurrentTransitionState = TransitionState.None;
        }
    }

    public void Update()
    {
        switch (CurrentTransitionState)
        {
            case TransitionState.TransitionIn:
                TransitionIn();
                break;

            case TransitionState.TransitionOut:
                TransitionOut();
                break;

            case TransitionState.None:
                // Do nothing or handle other logic if needed
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Debug.Assert(spriteBatch != null, "SpriteBatch cannot be null.");
        spriteBatch.Draw(FadeTexture, Vector2.Zero, Color.Black * TransitionAlpha);
    }
}