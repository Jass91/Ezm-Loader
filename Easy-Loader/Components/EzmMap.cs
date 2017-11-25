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
    public class EzmMap
    {
        public Dictionary<int, EzmTileSet> TileSets { get; private set; }

        public Dictionary<int, EzmTile> Tiles { get; private set; }

        public Dictionary<int, EzmLayer> Layers { get; private set; }

        public int WidthInPixels { get; private set; }

        public int HeightInPixels { get; private set; }

        public int Width { get;  private set; }

        public int Height { get;  private set; }

        public int TileWidth { get;  private set; }

        public int TileHeight { get;  private set; }

        public string Orientation { get;  private set; }             
        
        [JsonConstructor]
        private EzmMap(
            ICollection<EzmTileSet> TileSets, 
            ICollection<EzmTile> Tiles, 
            ICollection<EzmLayer> Layers,
            int width, int height,
            int tilewidth, int tileheight,
            string orientation)
        {
            Orientation = orientation;
            Width = width;
            Height = height;
            TileWidth = tilewidth;
            TileHeight = tileheight;

            WidthInPixels = width * tilewidth;
            HeightInPixels = height * tileheight;

            this.TileSets = TileSets.ToDictionary(tset => tset.ID, tset => tset, null);
            this.Tiles = Tiles.ToDictionary(t => t.ID, t => t);
            this.Layers = Layers.ToDictionary(l => l.ID, l => l);

        }

        public void Draw(SpriteBatch spriteBatch, int leftMargin = 0, int topMargin = 0)
        {
            if (Orientation == "orthogonal")
                DrawOrthogonal(spriteBatch, leftMargin, topMargin);
            else if (Orientation == "isometric")
                DrawIsometric(spriteBatch, leftMargin, topMargin);
        }

        private void DrawOrthogonal(SpriteBatch spriteBatch, int leftMargin = 0, int topMargin = 0)
        {
            spriteBatch.Begin();
            foreach (var l in Layers.Values.OrderBy(l => l.ID))
            {
                for (int i = 0; i < l.Data.Length; i++)
                {
                    var tile = Tiles[l.Data[i]];
                    if (tile == null)
                        continue;

                    var tilesetTexture = TileSets[tile.Tileset].Texture;
                    var col = i % l.Width;
                    var row = i / l.Height;

                    var screenLocation = new Rectangle((col * tile.Width) + leftMargin, (row * tile.Height) + topMargin, tile.Width, tile.Height);
                    var tileArea = new Rectangle(tile.TileCol * tile.Width, tile.TileRow * tile.Height, tile.Width, tile.Height);

                    spriteBatch.Draw(tilesetTexture, screenLocation, tileArea, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
            }

            spriteBatch.End();
        }

        private void DrawIsometric(SpriteBatch spriteBatch, int leftMargin = 0, int topMargin = 0)
        {

        }

        public void Unload()
        {
            // Unload tileset textures
            foreach (var tSet in TileSets.Values)
            {
                TileSets[tSet.ID].Texture.Dispose();
            }
        }
    }
}
