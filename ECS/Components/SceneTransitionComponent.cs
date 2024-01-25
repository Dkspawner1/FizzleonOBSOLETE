using Fizzleon.ECS.Systems;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        private TextureLoaderSystem textureLoaderSystem;

        public SceneTransitionComponent(TextureLoaderSystem textureLoaderSystem)
        {
            this.textureLoaderSystem = textureLoaderSystem;
            fadeTexture = textureLoaderSystem?.LoadTransitionTexture("Textures/btn0") ?? throw new ArgumentNullException(nameof(textureLoaderSystem));
            CurrentTransitionState = TransitionState.None;
        }

        public void TransitionIn()
        {
            CurrentTransitionState = TransitionState.TransitionIn;

            transitionAlpha = Math.Min(1f, transitionAlpha + transitionSpeed * (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds);

            if (transitionAlpha >= 1f)
            {
                transitionAlpha = 1f;
                CurrentTransitionState = TransitionState.None;
            }
        }


        public void TransitionOut()
        {
            CurrentTransitionState = TransitionState.TransitionOut;

            transitionAlpha = Math.Max(0f, transitionAlpha - transitionSpeed * (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds);

            if (transitionAlpha <= 0f)
            {
                transitionAlpha = 0f;
                CurrentTransitionState = TransitionState.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fadeTexture, new Rectangle(0, 0, Data.Window.Width, Data.Window.Height), new Color(255, 255, 255, (int)(255 * transitionAlpha)));

        }
    }
}
