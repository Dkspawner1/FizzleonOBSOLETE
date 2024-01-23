using Fizzleon.ECS.Components;
using Fizzleon.Network;
using Lidgren.Network;
using MonoGame.Extended.Entities;
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

        // Used for networking
        public Player(BindPlayerToNetwork networkPlayer)
        {
            networkPlayer = new BindPlayerToNetwork(this, networkPlayer.Connection);
            
        }

        public void Dispose() => entity.Destroy();


        public void LoadContent(Game instance,Texture2D texture, string pathToSF)
        {
            var sprite = new SpriteComponent(texture);
            var animation = new AnimationComponent(pathToSF, sprite.Texture);
            animation.LoadContent(instance.Content);
            
            sprite.SetTransform(entity.Get<TransformComponent>());
            animation.SetTransform(entity.Get<TransformComponent>());

            entity.Attach(sprite);
            entity.Attach(animation);
        }

        public void Update(GameTime gameTime, params string[] animationKeys)
        {
            UpdatePlayerAnimation(gameTime);
            UpdatePlayerMovement();

            // Update the player's position based on velocity
            var transform = entity.Get<TransformComponent>();
            var playerMovement = entity.Get<MovementComponent>();

            // Update the position using the velocity
            transform.Position += playerMovement.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            entity.Get<AnimationComponent>().Play(animationKeys.FirstOrDefault(), true, () => { });

        }

        private void UpdatePlayerAnimation(GameTime gameTime) => entity.Get<AnimationComponent>().Update(gameTime);

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

