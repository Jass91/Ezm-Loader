using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Collections.ObjectModel;
using Shared.Enums;
using Shared.Utils;

namespace SimpleExample.Core.Player
{
    public class Player
    {
        public Vector2 Position { get; private set; }

        public Dictionary<string, Texture2D> Textures;

        private float _velocity = 1.5f;

        private int _currentFrame;

        private float _scale = 1;

        private int _index;

        private bool _isMoving = false;

        private bool _grow = false;

        private Direction _dir = Direction.RIGHT;

        public Player(int index)
        {
            _index = index;
            _currentFrame = 2;
            _isMoving = false;

            Position = new Vector2(10, 10);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var textureKey = _dir.ToString() + _currentFrame;
            var texture = Textures[textureKey];
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Position, Color.White);            
            spriteBatch.End();
            _isMoving = false;
            _grow = false;
        }


        public void Animate()
        {
            if (_isMoving)
                _currentFrame = _currentFrame >= 0 && _currentFrame < 3 ? _currentFrame + 1 : 1;            
            else
                _currentFrame = 2;

            if (_grow)
                _scale += 1;
        }

        public void LoadTexture(Texture2D spritesheet)
        {
            Texture2D texture;
            var textures = new Dictionary<string, Texture2D>();
            var rowNumber = (spritesheet.Height / 32);
            var colNumber = (spritesheet.Width / 32);
            for (var i = 0; i < colNumber; i++)
            {
                for (var j = 0; j <= rowNumber; j++)
                {
                    var textureKey = ((Direction)j).ToString() + (i + 1);
                    var xRegion = (_index * 3 * 32) + (i * 32);
                    var yRegion = (j * 32);
                    texture = ImageUtils.Crop(spritesheet, new Rectangle(xRegion, yRegion, 32, 32));
                    textures.Add(textureKey, texture);
                }
            }

            Textures = textures;
        }

        public void UpdateDirection(Direction dir)
        {
            _dir = dir;
            _isMoving = true;
            if (dir == Direction.LEFT)
                Position = new Vector2(Position.X - _velocity, Position.Y);
            else if (dir == Direction.RIGHT)
                Position = new Vector2(Position.X + _velocity, Position.Y);
            else if (dir == Direction.UP)
                Position = new Vector2(Position.X, Position.Y - _velocity);
            else
                Position = new Vector2(Position.X, Position.Y + _velocity);     
        }

        public void Grow()
        {
            _grow = true;
        }
    }
}
