using Fizzleon.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Fizzleon.ECS.Components
{
    public class SceneTransitionComponent 
    {
        public enum TransitionState
        {
            None,
            TransitionIn,
            TransitionOut
        }

        public TransitionState CurrentTransitionState { get; private set; }

        private const float transitionSpeed = 0.1f;
        private float transitionAlpha = 0f;
        private Texture2D fadeTexture;
        private Game Instance;

        public SceneTransitionComponent(Game instance)
        {
            Instance = instance;

            fadeTexture = instance.Content.Load<Texture2D>("Textures/Warrior_Sheet-Effect");
            CurrentTransitionState = TransitionState.None;
        }

        public void TransitionIn()
        {
            CurrentTransitionState = TransitionState.TransitionIn;

            transitionAlpha += transitionSpeed * (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds;
            if (transitionAlpha >= 1f)
            {
                transitionAlpha = 1f;
                CurrentTransitionState = TransitionState.None;
            }
        }

        public void TransitionOut()
        {
            CurrentTransitionState = TransitionState.TransitionOut;

            transitionAlpha -= transitionSpeed * (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds;

            if (transitionAlpha <= 0f)
            {
                transitionAlpha = 0f;
                CurrentTransitionState = TransitionState.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fadeTexture, new Rectangle(0, 0, Instance.GraphicsDevice.Viewport.Width, Instance.GraphicsDevice.Viewport.Height), new Color(255, 255, 255, (int)(255 * transitionAlpha)));

        }
    }
}
