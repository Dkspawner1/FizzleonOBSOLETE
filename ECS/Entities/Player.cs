using Fizzleon.ECS.Components;
using Fizzleon.Network;
using Lidgren.Network;
using MonoGame.Extended.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fizzleon.ECS.Entities
{
    public class Player : IDisposable
    {
        public readonly Entity entity;

        // Used for game
        public Player(World world, Vector2 spawn, float speed)
        {
            entity = world.CreateEntity();
            entity.Attach(new TransformComponent(spawn));
            entity.Attach(new MovementComponent());
            entity.Attach(new PlayerComponent(speed));
        }

        public Player(BindPlayerToNetwork networkPlayer)
        {
            networkPlayer = new BindPlayerToNetwork(this, networkPlayer.Connection);
        }

        public void Dispose() => entity.Destroy();

        // Pass the Game1 instance to LoadContent
        public void LoadContent(TextureLoaderSystem textureLoaderSystem, string textureName, string pathToSF)
        {
            var loadedTexture = textureLoaderSystem.LoadTexture(entity, textureName);
            var sprite = new SpriteComponent(loadedTexture);

            var animation = new AnimationComponent(pathToSF, loadedTexture);
            animation.LoadContent(Data.Content);

            sprite.SetTransform(entity.Get<TransformComponent>());
            animation.SetTransform(entity.Get<TransformComponent>());

            entity.Attach(sprite);
            entity.Attach(animation);
        }

        public void Update(params string[] animationKeys)
        {
            UpdatePlayerAnimation();
            UpdatePlayerMovement();

            // Update the player's position based on velocity
            var transform = entity.Get<TransformComponent>();
            var playerMovement = entity.Get<MovementComponent>();

            // Update the position using the velocity
            transform.Position += playerMovement.Velocity * (float)Data.GameTime.ElapsedGameTime.TotalSeconds;

            entity.Get<AnimationComponent>().Play(animationKeys.FirstOrDefault(), true, () => { });
        }

        private void UpdatePlayerAnimation() => entity.Get<AnimationComponent>().Update();

        private void UpdatePlayerMovement()
        {
            var playerMovement = entity.Get<MovementComponent>();
            var playerSpeed = entity.Get<PlayerComponent>().Speed;

            // Get the current state of the keyboard
            var currentKeyboardState = Keyboard.GetState();

            // Determine the horizontal movement based on keyboard input
            float horizontalMovement = 0;

            if (currentKeyboardState.IsKeyDown(Keys.Left))
                horizontalMovement -= 1;

            if (currentKeyboardState.IsKeyDown(Keys.Right))
                horizontalMovement += 1;

            // Set the velocity based on the horizontal movement
            playerMovement.Velocity = new Vector2(horizontalMovement * playerSpeed, 0);
        }
    }
}
