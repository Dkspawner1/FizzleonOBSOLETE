using Fizzleon.Events.Args;
using Fizzleon.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using static Fizzleon.Core.Data.GameState;

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
     

        public TransitionState CurrentTransitionState { get; set; } = TransitionState.None;

        public float TransitionAlpha { get; private set; } = 0f;

        public Texture2D FadeTexture { get; set; }

        public const float TransitionSpeed = 1.0f / 7.0f;
        private bool isTransitionOutComplete = false;

        public void LoadContent(Texture2D texture)
        {
            FadeTexture = texture;
        }

        public void TransitionIn()
        {
            float elapsedTime = (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds;
            TransitionAlpha = MathHelper.Lerp(TransitionAlpha, 1f, TransitionSpeed * elapsedTime);

            if (TransitionAlpha >= 0.99f) // Close enough to 1, consider it fully transitioned in
            {
                TransitionAlpha = 1f;
                CurrentTransitionState = TransitionState.None;
            }
        }
        
        public event EventHandler<TransitionEventArgs> TransitionOutComplete;

        public void TransitionOut(IScene currentScene)
        {
            float elapsedTime = (float)Data.GameTime.ElapsedGameTime.TotalMilliseconds;

            Trace.WriteLine($"TransitionOut: Current Alpha: {TransitionAlpha}");

            TransitionAlpha = MathHelper.Lerp(TransitionAlpha, 0f, TransitionSpeed * elapsedTime);

            Trace.WriteLine($"TransitionOut: Updated Alpha: {TransitionAlpha}");

            if (TransitionAlpha <= 0.01f && !isTransitionOutComplete)
            {
                TransitionAlpha = 0f;
                CurrentTransitionState = TransitionState.None;
                Trace.WriteLine("TransitionOut complete");

                // Pass the current scene to the OnTransitionOutComplete method
                OnTransitionOutComplete(currentScene);

                isTransitionOutComplete = true;
            }
        }


        private void OnTransitionOutComplete(IScene currentScene)
        {
            string sceneChangeMessage = "Transitioning out complete"; // Provide an appropriate scene change message
            string previousSceneId = "PreviousSceneId"; // Provide the correct previous scene ID or obtain it from your context
            string newSceneId = "NewSceneId"; // Provide the correct new scene ID or obtain it from your context

            TransitionOutComplete?.Invoke(this, new TransitionEventArgs(
                TransitionState.TransitionOut,
                TransitionState.None,
                sceneChangeMessage,
                previousSceneId,
                newSceneId
            ));
        }

        public void Update(GameTime gameTime)
        {
            switch (CurrentTransitionState)
            {
                case TransitionState.TransitionIn:
                    TransitionIn();
                    break;

                case TransitionState.TransitionOut:
                    TransitionOut(Data.CurrentScene);
                    break;

                case TransitionState.None:
                    break;
            }
        }

        public void DrawTransition(SpriteBatch spriteBatch)
        {
            Debug.Assert(spriteBatch != null, "SpriteBatch cannot be null.");

            // Draw only if TransitionAlpha is not zero
            if (TransitionAlpha > 0f)
            {
                spriteBatch.Draw(FadeTexture, Vector2.Zero, Color.Black * TransitionAlpha);
            }
        }
    }
}
