namespace Fizzleon.ECS.Components
{
    public class TransitionComponent
    {
        public enum TransitionState
        {
            None,
            TransitionIn,
            TransitionOut
        }

        public TransitionState CurrentTransitionState { get; private set; }
        private const float TransitionSpeed = 0.1f;
        private float transitionAlpha = 0f;
        private readonly Texture2D fadeTexture;
        private readonly Game1 instance;

        public TransitionComponent(Game1 instance, string texturePath)
        {
            this.instance = instance;
            fadeTexture = instance.Content.Load<Texture2D>(texturePath);
            CurrentTransitionState = TransitionState.None;
        }

        public void StartTransitionIn()
        {
            CurrentTransitionState = TransitionState.TransitionIn;
        }

        public void StartTransitionOut()
        {
            CurrentTransitionState = TransitionState.TransitionOut;
        }

        public void Update(GameTime gameTime)
        {
            float elapsedMilliseconds = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTransitionState == TransitionState.TransitionIn)
            {
                transitionAlpha += TransitionSpeed * elapsedMilliseconds;

                if (transitionAlpha >= 1f)
                {
                    transitionAlpha = 1f;
                    CurrentTransitionState = TransitionState.None;
                }
            }
            else if (CurrentTransitionState == TransitionState.TransitionOut)
            {
                transitionAlpha -= TransitionSpeed * elapsedMilliseconds;

                if (transitionAlpha <= 0f)
                {
                    transitionAlpha = 0f;
                    CurrentTransitionState = TransitionState.None;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                fadeTexture,
                new Rectangle(0, 0, instance.GraphicsDevice.Viewport.Width, instance.GraphicsDevice.Viewport.Height),
                new Color(255, 255, 255, (int)(255 * transitionAlpha))
            );
        }

        public void DisposeContent()
        {
            fadeTexture.Dispose();
        }
    }
}
