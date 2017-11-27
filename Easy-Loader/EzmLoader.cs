using EzmLoader.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Shared.Utils;
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
        internal static EzmMap LoadMap(string pathToEzmFile, string pathTotileSetFolder, ContentManager content)
        {
            EzmMap map;

            // load map from file
            using (StreamReader r = new StreamReader(pathToEzmFile))
            {
                string json =  r.ReadToEnd();
                map = JsonConvert.DeserializeObject<EzmMap>(json);
            }

            // load tileset textures
            var tileSetsTextures = new Dictionary<int, Texture2D>();
            foreach (var tSet in map.TileSets.Values)
            {
                var tileSetAssetID =  tSet.Source.Substring(tSet.Source.IndexOf(pathTotileSetFolder));
                tileSetAssetID = tileSetAssetID.Substring(0, tileSetAssetID.LastIndexOf("."));

                var texture = content.Load<Texture2D>(tileSetAssetID);

                // apagar depois
                map.TileSets[tSet.ID].Texture = texture;

                tileSetsTextures.Add(tSet.ID, texture);
            }

            // set row, column and color of each tile
            foreach (var layer in map.Layers.Values)
            {
                foreach (var t in layer.Data2)
                {
                    if (t.ID < 0)
                        continue;

                    var tileSetTexture = tileSetsTextures[t.Tileset];
                    if (tileSetTexture == null)
                        continue;

                    var tileCol = t.TileOrder % (tileSetTexture.Width / t.Width);
                    var tileRow = t.TileOrder / (tileSetTexture.Width / t.Width);
                    var tileArea = new Rectangle(tileCol * t.Width, tileRow * t.Height, t.Width, t.Height);

                    t.Texture = ImageUtils.Crop(tileSetTexture, tileArea);
                }                
            }

            // apagar depois
            // set row, column and color of each tile
            //foreach (var t in map.Tiles.Values)
            //{
            //    var tileSet = map.TileSets[t.Tileset];
            //    if (tileSet == null)
            //        continue;
                 
            //    t.TileCol = t.TileOrder % (tileSet.Width / t.Width);
            //    t.TileRow = t.TileOrder / (tileSet.Width / t.Width);
            //    t.Color = Color.White;
                
            //}

            return map; 
        }
    }
}
