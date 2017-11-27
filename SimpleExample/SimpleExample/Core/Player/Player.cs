using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Shared.Enums;
using Microsoft.Xna.Framework.Input;
using EzmLoader.Shared.Utils;

namespace SimpleExample.Core.Player
{
    public class Player
    {
        public Vector2 Position { get; private set; }

        public Dictionary<string, Texture2D> Textures;

        private float _velocity = 1.5f;

        private int _currentFrame;

        private int _index;

        private bool _isMoving = false;

        private Direction _dir = Direction.RIGHT;

        public Player(int index)
        {
            _index = index;
            _currentFrame = 2;
            _isMoving = false;

            Position = new Vector2(32, 32);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var textureKey = _dir.ToString() + _currentFrame;
            var texture = Textures[textureKey];
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Position, Color.White);            
            spriteBatch.End();
        }

        public void Animate()
        {
            if (_isMoving)
            {
                _currentFrame = _currentFrame >= 0 && _currentFrame < 3 ? _currentFrame + 1 : 1;
                _isMoving = false;
            }
            else
                _currentFrame = 2;
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

        public void MoveTo(Vector2 nextPosition)
        {
            Position = nextPosition;
            _isMoving = true;
        }

        public Vector2 GetNextPostion()
        {
            Vector2 nextPos = Position;
          
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
            {
                _dir = Direction.UP;
                nextPos = new Vector2(Position.X, Position.Y - _velocity);
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
            {
                _dir = Direction.LEFT;
                nextPos = new Vector2(Position.X - _velocity, Position.Y);
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S))
            {
                _dir = Direction.DOWN;
                nextPos = new Vector2(Position.X, Position.Y + _velocity);
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
            {
                _dir = Direction.RIGHT;
                nextPos = new Vector2(Position.X + _velocity, Position.Y);
            }

            return nextPos;
        }
    }
}
