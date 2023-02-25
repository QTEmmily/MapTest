using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TutorialMonoGame.Core;
using MonoGame.Extended;

namespace TutorialMonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const float PIXELS_PER_SECOND = 100.0f;
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D playerTexture;

        private Texture2D backgroundTexture;

        private Rectangle mapsize;
        private Rectangle playersize;

        private Vector2 playerPosition;

        private TimeSpan previousGametime = new(0L);

        public Game1()
        {
            graphics = new(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {base.Initialize();}

        protected override void LoadContent()
        {
            spriteBatch = new(graphics.GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("ball");
            backgroundTexture = Content.Load<Texture2D>("test_map");

            mapsize = new(-backgroundTexture.Width/2, -backgroundTexture.Height/2,
                          backgroundTexture.Width, backgroundTexture.Height);
            playersize = new(-playerTexture.Width/2, -playerTexture.Height/2,
                             playerTexture.Width, playerTexture.Height);

            playerPosition = new(0,0);
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            if (previousGametime.Ticks == 0L) {
                previousGametime = gameTime.TotalGameTime;
            }

            float deltaSeconds = (float)(gameTime.TotalGameTime - previousGametime).Ticks / (float)TimeSpan.TicksPerSecond;
            previousGametime = gameTime.TotalGameTime;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                playerPosition.Y -= deltaSeconds*PIXELS_PER_SECOND;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerPosition.Y += deltaSeconds*PIXELS_PER_SECOND;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerPosition.X -= deltaSeconds*PIXELS_PER_SECOND;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerPosition.X += deltaSeconds*PIXELS_PER_SECOND;
            }

            //Don't allow player to move outside of map
            if ((playerPosition.X + playersize.Left) < mapsize.Left) {
                playerPosition.X = mapsize.Left - playersize.Left;
            } else if ((playerPosition.X + playersize.Right) > mapsize.Right) {
                playerPosition.X = mapsize.Right - playersize.Right;
            }
            if ((playerPosition.Y + playersize.Top) < mapsize.Top) {
                playerPosition.Y = mapsize.Top - playersize.Top;
            } else if ((playerPosition.Y + playersize.Bottom) > mapsize.Bottom) {
                playerPosition.Y = mapsize.Bottom - playersize.Bottom;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Viewport viewport = graphics.GraphicsDevice.Viewport;

            Vector2 centreOfViewport = new(viewport.X + viewport.Width/2, viewport.Y + viewport.Height/2);
            Vector2 mapPosUpperLeftOnViewport = new(centreOfViewport.X + mapsize.Left, centreOfViewport.Y + mapsize.Top);
            Vector2 playerPosUpperLeftOnViewport = new(centreOfViewport.X + playersize.Left, centreOfViewport.Y + playersize.Top);

            //Move map relative to player being in centre
            mapPosUpperLeftOnViewport -= playerPosition;

            //Don't allow map to move outside of viewport
            Vector2 viewportOffset = new(0, 0);
            if (mapPosUpperLeftOnViewport.X > viewport.X) {
                viewportOffset.X = mapPosUpperLeftOnViewport.X - viewport.X;
            } else if ((mapPosUpperLeftOnViewport.X+mapsize.Width) < (viewport.X+viewport.Width)) {
                viewportOffset.X = (mapPosUpperLeftOnViewport.X+mapsize.Width) - (viewport.X+viewport.Width);
            }
            if (mapPosUpperLeftOnViewport.Y > viewport.Y) {
                viewportOffset.Y = mapPosUpperLeftOnViewport.Y - viewport.Y;
            } else if ((mapPosUpperLeftOnViewport.Y+mapsize.Height) < (viewport.Y+viewport.Height)) {
                viewportOffset.Y = (mapPosUpperLeftOnViewport.Y+mapsize.Height) - (viewport.Y+viewport.Height);
            }
            mapPosUpperLeftOnViewport -= viewportOffset;
            playerPosUpperLeftOnViewport -= viewportOffset;
  

            //Draw stuff

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture,
                             mapPosUpperLeftOnViewport,
                             Color.White);

            spriteBatch.Draw(playerTexture,
                             playerPosUpperLeftOnViewport,
                             Color.Green);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}