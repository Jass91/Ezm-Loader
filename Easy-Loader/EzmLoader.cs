using EzmLoader;
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
        internal static EzmMap LoadMap(string pathToEzmFile, string pathTotileSetFolder, ContentManager content)
        {
            EzmMap map;

            try
            {
                // load map from file
                using (StreamReader r = new StreamReader(pathToEzmFile))
                {
                    string json = r.ReadToEnd();
                    map = JsonConvert.DeserializeObject<EzmMap>(json);
                }

                // set addition info to tilesets
                var tileSetsTextures = new Dictionary<int, Texture2D>();
                foreach (var tSet in map.TileSets.Values)
                {
                    var tileSetAssetID = tSet.Source.Substring(tSet.Source.IndexOf(pathTotileSetFolder));
                    tileSetAssetID = tileSetAssetID.Substring(0, tileSetAssetID.LastIndexOf("."));

                    // store tileset image
                    map.TileSets[tSet.ID].Texture = content.Load<Texture2D>(tileSetAssetID);
                }

                // set addition info to tiles
                foreach (var layer in map.Layers.Values)
                {
                    foreach (var t in layer.Data)
                    {
                        if (t == null)
                            continue;

                        var tileSetTexture = map.TileSets[t.Tileset.Value].Texture;
                        if (tileSetTexture == null)
                        {
                            throw new Exception($"Tile {t.ID} uses an invalid tileset {t.Tileset.Value}");
                        }

                        var tileCol = t.TileOrder.Value % (tileSetTexture.Width / t.Width);
                        var tileRow = t.TileOrder.Value / (tileSetTexture.Width / t.Width);
                        var tileArea = new Rectangle(tileCol * t.Width, tileRow * t.Height, t.Width, t.Height);

                        // set tile area to crop from tileset
                        layer.Data[t.Row, t.Column].TileSetArea = tileArea;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return map; 
        }
    }
}
