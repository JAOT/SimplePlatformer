using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleGame
{
    class Tile : Jumper
    {
        public bool IsBlocked { get; set; }

        public Tile(Texture2D texture, Vector2 position, bool isBlocked, SpriteBatch batch) : base(texture, position, batch)
        {
            IsBlocked = isBlocked;
        }

        public override void Draw()
        {
            if (IsBlocked)
            {
                base.Draw();
            }
        }
    }
}
