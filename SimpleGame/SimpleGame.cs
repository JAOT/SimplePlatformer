using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SimpleGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont DebugFont { get; set; }
        public Texture2D JumperTexture { get; private set; }
        public Texture2D TileTexture { get; private set; }
        public Jumper Jumper { get; private set; }
        Board Board { get; set; }
        public SimpleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 640;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            DebugFont = Content.Load<SpriteFont>("DebugFont");
            JumperTexture = Content.Load<Texture2D>("Jumper");
            TileTexture = Content.Load<Texture2D>("Tile");

            Jumper = new Jumper(JumperTexture, new Vector2(80, 80), spriteBatch);

            Board = new Board(10, 15, TileTexture, spriteBatch);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Jumper.Update(gameTime);
            CheckKeyboardAndReact();
            base.Update(gameTime);
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) { RestartGame(); }
        }

        private void RestartGame()
        {
            Board.CurrentBoard.CreateNewBoard();
            PutJumperInTopLeftCorner();
        }

        private void PutJumperInTopLeftCorner()
        {
            Jumper.Position = Vector2.One * 80;
            Jumper.Movement = Vector2.Zero;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            Board.Draw();
            WriteDebugInformation();
            Jumper.Draw();

            base.Draw(gameTime);
            spriteBatch.End();
        }

        private void WriteDebugInformation()
        {
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", Jumper.Position.X, Jumper.Position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", Jumper.Movement.X, Jumper.Movement.Y);
            string isOnFirmGroundText = string.Format("On firm ground? : {0}", Jumper.IsOnFirmGround());
            spriteBatch.DrawString(DebugFont, positionInText, new Vector2(10, 0), Color.White);
            spriteBatch.DrawString(DebugFont, movementInText, new Vector2(10, 20), Color.White);
            spriteBatch.DrawString(DebugFont, isOnFirmGroundText, new Vector2(10, 40), Color.White);
        }
    }
}
