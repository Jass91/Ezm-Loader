using EzmLoader;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleExample.Core.Map
{

    public class Map
    {
        private EzmMap _map;
        private ContentManager _content;
    
        public int Width { get { return _map.Width; } private set { } }

        public int Height { get { return _map.Height; } private set { } }


        public Map(ContentManager content, string pathToEzmMap)
        {
            _content = content;
            _map = EzmLoader.EzmLoader.LoadMap(pathToEzmMap, "TileSets", content);
        }

        public void Draw(SpriteBatch spriteBatch, int leftMargin = 0, int topMargin = 0)
        {
            _map.Draw(spriteBatch, leftMargin, topMargin);
        }

        public void Unload()
        {
            _map.Unload();
        }   
    }
}
