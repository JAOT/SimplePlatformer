using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleGame
{
    public class Jumper : Sprite
    {
        public Vector2 Movement { get; set; }
        Vector2 OldPosition { get; set; }
        public Jumper(Texture2D texture, Vector2 position, SpriteBatch batch) : base(texture, position, batch)
        {
        }

        public void Update(GameTime gameTime)
        {

            CheckKeyboardAndUpdateMovement();
            AffectWithGravity();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
        }

        private void AffectWithGravity()
        {
            Movement += Vector2.UnitY * 0.5f;
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            OldPosition = Position;

            UpdatePositionBasedOnMovement();

            Position = Board.CurrentBoard.WhereCanIGetTo(OldPosition, Position, Bounds);
        }

        private void UpdatePositionBasedOnMovement()
        {
            Position += Movement;
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left)) { Movement += new Vector2(-1, 0); }
            if (keyboardState.IsKeyDown(Keys.Right)) { Movement += new Vector2(1, 0); }
            if (keyboardState.IsKeyDown(Keys.Up)&&IsOnFirmGround()) { Movement = -Vector2.UnitY * 25; }
        }

        private void SimulateFriction()
        {
            Movement -= Movement * new Vector2(.1f, .1f);
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = Bounds;
            onePixelLower.Offset(0, 1);
            return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - OldPosition;
            if (lastMovement.X == 0)
            {
                Movement *= Vector2.UnitY;
            }
            if (lastMovement.Y == 0)
            {
                Movement *= Vector2.UnitX;
            }
        }
    }
}
