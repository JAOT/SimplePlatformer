using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame
{
    class Board
    {
        Tile[,] Tiles { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        Texture2D TileTexture { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        public static Board CurrentBoard { get; private set; }

        public Board(int rows, int columns, Texture2D tileTexture, SpriteBatch spriteBatch)
        {
            Rows = rows;
            Columns = columns;
            TileTexture = tileTexture;
            SpriteBatch = spriteBatch;
            Tiles = new Tile[Columns, Rows];
            CreateNewBoard();
            CurrentBoard = this;
        }

        private void InitializeAllTilesAndBlockSomeRandomly()
        {
            Random Random = new Random();
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition =
                        new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] =
                        new Tile(TileTexture, tilePosition, Random.Next(5) == 0, SpriteBatch);
                }
            }
        }

        private void SetTopLeftTileUnblocked()
        {
            Tiles[1, 1].IsBlocked = false;
        }

        private void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if(x==0||x==Columns-1||y==0||y==Rows-1)
                    {
                        Tiles[x, y].IsBlocked = true;
                    }
                }
            }
        }

        public void Draw()
        {
            //for (int x = 0; x < Columns; x++)
                //{
                //    for (int y = 0; y < Rows; y++)
                //    {
                //        Tiles[x, y].Draw();
                //    }
                //}
                foreach (var tile in Tiles)
                {
                    tile.Draw();
                }
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.IsBlocked&&tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
        }

        internal void CreateNewBoard()
        {
            InitializeAllTilesAndBlockSomeRandomly();
            SetAllBorderTilesBlocked();
            SetTopLeftTileUnblocked();
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            Vector2 movementToTry = destination - originalPosition;
            Vector2 furthestAvailableLocationSoFar = originalPosition;
            int numberOfStepsToBreakMovementInto = ((int)movementToTry.Length() * 2) + 1;

            Vector2 oneStep = movementToTry / numberOfStepsToBreakMovementInto;

            for (int i = 0; i <= numberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + oneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary))
                {
                    furthestAvailableLocationSoFar = positionToTry;
                }
                else
                {
                    bool isDiagonalMove = movementToTry.X != 0 && movementToTry.Y != 0;
                    if (isDiagonalMove)
                    {
                        int stepsLeft = numberOfStepsToBreakMovementInto - (i - 1);

                        Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * stepsLeft;
                        Vector2 finalPositionIfMovingHorizontally = furthestAvailableLocationSoFar + remainingHorizontalMovement;
                        furthestAvailableLocationSoFar =
                            WhereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingHorizontally, boundingRectangle);

                        Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * stepsLeft;
                        Vector2 finalPositionIfMovingVertically = furthestAvailableLocationSoFar + remainingVerticalMovement;
                        furthestAvailableLocationSoFar =
                            WhereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingVertically, boundingRectangle);
                    }
                    break;
                }
            }
            return furthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }
    }
}