using EzmLoader;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleExample.Core.Map
{

    public class Map : EzmMap
    {
        public Map(ContentManager content, string pathToEzmFile, string pathToTilesetsFolders) :
            base(content, pathToEzmFile, pathToTilesetsFolders) { }
          
    }
}
