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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera _camera;

        private Texture2D _playerTexture;

        private Texture2D _backgroundTexture;

        private SpriteFont _spriteFont;

        private Vector2 _mapposition = new Vector2(400, 240);
        private Vector2 _playerPosition;

        private Rectangle _border;
        private Rectangle _topborder;
        private Rectangle _rightborder;
        private Rectangle _bottomborder;
        private Rectangle _leftborder;
        private Rectangle _endborder;

        private bool _playercheck;
        private bool _mapcheck;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _camera = new Camera();

            _border = new Rectangle(-1250,-1410,2500,2820);
            _topborder = new Rectangle(-1600, -1600, 3200, 240);
            _rightborder = new Rectangle(1200, -1600, 400, 3200);
            _bottomborder = new Rectangle(-1600, 1360, 3200, 240);
            _leftborder = new Rectangle(-1600, -1600, 400, 3200);
            _endborder = new Rectangle(-1150, -1310, 2300, 2620);

            _playerTexture = Content.Load<Texture2D>("ball");
            _backgroundTexture = Content.Load<Texture2D>("test_map");
            _spriteFont = Content.Load<SpriteFont>("File");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float xspeed = 0f;
            float yspeed = 0f;
            if(!_topborder.Contains(_playerPosition) && !_bottomborder.Contains(_playerPosition))
            {
                yspeed = 1f;
            }
            if (!_rightborder.Contains(_playerPosition) && !_leftborder.Contains(_playerPosition))
            {
                xspeed = 1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && !_bottomborder.Contains(_playerPosition))
            {
                _playerPosition.Y -= 3f * yspeed;
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.W) && _topborder.Contains(_playerPosition))
            {
                _mapposition.Y -= 3f * yspeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _playerPosition.Y -= 3f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && !_topborder.Contains(_playerPosition))
            {
                _playerPosition.Y += 3f * yspeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _playerPosition.Y += 3f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) && !_rightborder.Contains(_playerPosition))
            {
                _playerPosition.X -= 3f * xspeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _playerPosition.X -= 3f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) && !_leftborder.Contains(_playerPosition))
            {
                _playerPosition.X += 3f * xspeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _playerPosition.X += 3f;
            }





            if (_border.Contains(_playerPosition))
            {
                _playercheck = true;
            }
            else
            {
                _playercheck = false;
            }
            //if (!_endborder.Contains(_playerPosition))
            //{
            //    if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.D))
            //    {
            //        _playerPosition.X = -_playerPosition.X;
            //    }
            //    if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S))
            //    {
            //        _playerPosition.Y = -_playerPosition.Y;
            //    }
            //}

            if (_playercheck)
            {
                _camera.Follow(_playerPosition);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: _camera.Transform);

            spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), new Rectangle(0, 0, 1600, 1600), Color.White, 0f, new Vector2(800, 800), 2f, SpriteEffects.None, 0f);

            spriteBatch.DrawRectangle(_border, Color.DarkViolet, 5f);
            spriteBatch.DrawRectangle(_endborder, Color.Red, 5f);

            spriteBatch.DrawRectangle(_topborder, Color.Purple, 5f);
            spriteBatch.DrawRectangle(_rightborder, Color.Pink, 5f);
            spriteBatch.DrawRectangle(_bottomborder, Color.Orange, 5f);
            spriteBatch.DrawRectangle(_leftborder, Color.RoyalBlue, 5f);

            spriteBatch.End();

            spriteBatch.Begin();

            if (_mapcheck)
            {
                spriteBatch.Draw(_playerTexture, new Vector2(_playerPosition.X, _playerPosition.Y), Color.Green);
            }
            else if(_playercheck)
            {
                spriteBatch.Draw(_playerTexture, new Vector2(200, 240), Color.Green);
            }
            spriteBatch.Draw(_playerTexture, new Vector2(400, _mapposition.Y), Color.Red);

            

            spriteBatch.DrawString(_spriteFont, Convert.ToString(_playercheck), new Vector2(100, 100), Color.Crimson);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}