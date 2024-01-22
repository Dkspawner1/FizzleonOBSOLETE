using System.Diagnostics;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;

namespace Fizzleon.ECS.Entities
{
    public class Player
    {
        private readonly Entity player;
        private AnimationInitializationSystem animationInitializationSystem;
        private AnimationUpdateSystem animationUpdateSystem;

        public Player(World world)
        {
            player = world.CreateEntity();
            player.Attach(new TransformComponent(new Vector2(0, 0)));
            player.Attach(new MovementComponent());
            player.Attach(new PlayerComponent() { Speed = 500f });

            InitializeSystems();
        }
        private void InitializeSystems()
        {
            animationInitializationSystem = new AnimationInitializationSystem();
            animationUpdateSystem = new AnimationUpdateSystem(Game1.Instance.Content);
        }


        public void LoadContent(ContentManager Content)
        {
            Texture2D playerTexture = Content.Load<Texture2D>("Textures/Warrior_Sheet-Effect");

            var sprite = new SpriteComponent(playerTexture);
            var animation = new AnimationComponent("Textures/Warrior_Sheet-Effect.sf", sprite.Texture);

            animation.LoadContent(Content);

            sprite.SetTransform(player.Get<TransformComponent>());
            animation.SetTransform(player.Get<TransformComponent>());

            // Player's Spawn
            player.Get<TransformComponent>().Position = new Vector2(100, 100);

            player.Attach(sprite);
            player.Attach(animation);
        }

        public void Update(GameTime gameTime)
        {
            UpdatePlayerAnimation(gameTime);
            UpdatePlayerMovement();

            // Update the player's position based on velocity
            var transform = player.Get<TransformComponent>();
            var playerMovement = player.Get<MovementComponent>();

            // Update the position using the velocity
            transform.Position += playerMovement.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.Get<AnimationComponent>().Play("idleRight", true, () => { });
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
            {
                horizontalMovement -= 1;
                Trace.WriteLine("Moving left");
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                horizontalMovement += 1;
                Trace.WriteLine("Moving right");
            }

            // Set the velocity based on the horizontal movement
            playerMovement.Velocity = new Vector2(horizontalMovement * playerSpeed, 0);
        }


    }
}

