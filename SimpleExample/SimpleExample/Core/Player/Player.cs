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

        public int Width { get; set; }

        public int Height { get; set; }

        public Vector2 Position { get; private set; }

        public float Velocity { get; set; } = 1f;

        private Dictionary<string, Texture2D> _textures;

        private int _currentFrame;

        private int _index;

        private bool _isMoving = false;

        public Direction Direction { get; set; }
 
        public Player(int index)
        { 
            _index = index;
            _currentFrame = 2;
            _isMoving = false;

            Width = 32;
            Height = 32;
            Direction = Direction.RIGHT;
            Position = new Vector2(3 * 32, 1 * 32);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var textureKey = Direction.ToString() + _currentFrame;
            var texture = _textures[textureKey];
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

            _textures = textures;
        }

        public void MoveTo(Vector2 nextPosition)
        {
            var canMove = Keyboard.GetState().GetPressedKeys().Length > 0;
            if(canMove)
            {
                Position = nextPosition;
                _isMoving = true;
            }
        }

        public Vector2 GetNextPostion()
        {
            Vector2 nextPos = new Vector2();
            switch (Direction)
            {
                case Direction.UP:
                    nextPos = new Vector2(Position.X, Position.Y - Velocity);
                    break;
                case Direction.LEFT:
                    nextPos = new Vector2(Position.X - Velocity, Position.Y);
                    break;
                case Direction.DOWN:
                    nextPos = new Vector2(Position.X, Position.Y + Velocity);
                    break;
                case Direction.RIGHT:
                    nextPos = new Vector2(Position.X + Velocity, Position.Y);
                    break;
            }

            return nextPos;
        }
    }
}
