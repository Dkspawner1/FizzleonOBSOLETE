using Fizzleon.ECS.Components;
using Fizzleon.Network;
using Lidgren.Network;
using MonoGame.Extended.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Fizzleon.ECS.Systems;

namespace Fizzleon.ECS.Entities
{
    public class Player : IDisposable
    {
        private readonly Vector2 spawn;
        public readonly Entity Entity;

        public Player(World world, Vector2 spawn, float speed)
        {
            this.spawn = spawn;
            Entity = world.CreateEntity();
            
            Entity.Attach(new MovementComponent());
            Entity.Attach(new PlayerComponent(speed));
            
        }



        public void LoadContent(TextureLoaderSystem textureLoaderSystem, string pathToXnb, string pathToSf)
        {
            var animation = new AnimationComponent(textureLoaderSystem,pathToXnb, pathToSf);
            var animationSprite = new SpriteComponent(animation.Texture);
            animation.Transform = new TransformComponent(spawn);

            Entity.Attach(animationSprite);
            Entity.Attach(animation);
            Entity.Attach(animation.Transform);

        }

        public void Update(params string[] animationKeys)
        {
            UpdatePlayerAnimation();
            UpdatePlayerMovement();

            var transform = Entity.Get<TransformComponent>();
            var playerMovement = Entity.Get<MovementComponent>();

            transform.Position += playerMovement.Velocity * (float)Data.GameTime.ElapsedGameTime.TotalSeconds;

            Entity.Get<AnimationComponent>().Play(animationKeys.FirstOrDefault(), () => { });
        }

        private void UpdatePlayerAnimation() => Entity.Get<AnimationComponent>().Update();


        private void UpdatePlayerMovement()
        {
            var playerMovement = Entity.Get<MovementComponent>();
            var playerSpeed = Entity.Get<PlayerComponent>().Speed;
            var currentKeyboardState = Keyboard.GetState();
            float horizontalMovement = 0;

            if (currentKeyboardState.IsKeyDown(Keys.Left))
                horizontalMovement -= 1;

            if (currentKeyboardState.IsKeyDown(Keys.Right))
                horizontalMovement += 1;

            playerMovement.Velocity = new Vector2(horizontalMovement * playerSpeed, 0);
        }
        public void Dispose() => Entity.Destroy();

    }
}
