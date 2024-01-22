using System.Diagnostics;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;

namespace Fizzleon.ECS.Entities
{
    public class Player
    {
        private readonly Entity player;

        public Player(World world, Vector2 spawn, float speed)
        {
            player = world.CreateEntity();
            player.Attach(new TransformComponent(spawn));
            player.Attach(new MovementComponent());
            player.Attach(new PlayerComponent(speed));
        }
        public void LoadContent(ContentManager Content, Texture2D texture, string pathToSF)
        {
            var sprite = new SpriteComponent(texture);
            var animation = new AnimationComponent(pathToSF, sprite.Texture);

            animation.LoadContent(Content);

            sprite.SetTransform(player.Get<TransformComponent>());
            animation.SetTransform(player.Get<TransformComponent>());

            player.Attach(sprite);
            player.Attach(animation);
        }

        public void Update(GameTime gameTime, params string[] animationKeys)
        {
            UpdatePlayerAnimation(gameTime);
            UpdatePlayerMovement();

            // Update the player's position based on velocity
            var transform = player.Get<TransformComponent>();
            var playerMovement = player.Get<MovementComponent>();

            // Update the position using the velocity
            transform.Position += playerMovement.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.Get<AnimationComponent>().Play(animationKeys.FirstOrDefault(), true, () => { });

        }

        private void UpdatePlayerAnimation(GameTime gameTime) => player.Get<AnimationComponent>().Update(gameTime);

        private void UpdatePlayerMovement()
        {
            var playerMovement = player.Get<MovementComponent>();
            var playerSpeed = player.Get<PlayerComponent>().Speed;

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

