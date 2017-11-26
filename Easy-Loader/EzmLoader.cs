using EzmLoader.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzmLoader
{
    public static class EzmLoader
    {
        public static EzmMap LoadMap(string pathToEzmFile, string pathTotileSetFolder, ContentManager content)
        {
            EzmMap map;

            // load map from file
            using (StreamReader r = new StreamReader(pathToEzmFile))
            {
                string json =  r.ReadToEnd();
                map = JsonConvert.DeserializeObject<EzmMap>(json);
            }
          
            // load tileset textures
            foreach (var tSet in map.TileSets.Values)
            {
                var tileSetAssetID =  tSet.Source.Substring(tSet.Source.IndexOf(pathTotileSetFolder));
                tileSetAssetID = tileSetAssetID.Substring(0, tileSetAssetID.LastIndexOf("."));

                var texture = content.Load<Texture2D>(tileSetAssetID);
                map.TileSets[tSet.ID].Texture = texture;
            }

            // set row, column and color of each tile
            foreach (var t in map.Tiles.Values)
            {
                var tileSet = map.TileSets[t.Tileset];
                if (tileSet == null)
                    continue;
                 
                t.TileCol = t.TileOrder % (tileSet.Width / t.Width);
                t.TileRow = t.TileOrder / (tileSet.Width / t.Width);
                t.Color = Color.White;
            }

            return map; 
        }
    }
}
