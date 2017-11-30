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
    public class EzmMap : IDisposable
    {
        private Texture2D pixel;

        protected ContentManager Content { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; private set; }

        public Dictionary<string, EzmLayer> Layers { get; private set; }

        public Dictionary<int, EzmTileSet> TileSets { get; private set; }

        public int WidthInPixels { get; private set; }

        public int HeightInPixels { get; private set; }

        public int Width { get;  private set; }

        public int Height { get;  private set; }

        public int TileWidth { get;  private set; }

        public int TileHeight { get;  private set; }

        public string Orientation { get;  private set; }             
        
        public Vector2 Origin { get; private set; }

        public bool ShowTileBorder { get; set; }


        [JsonConstructor]
        private EzmMap
        (
            ICollection<EzmTileSet> TileSets, 
            ICollection<EzmTile> Tiles, 
            ICollection<EzmLayer> Layers,
            int width, int height,
            int tilewidth, int tileheight,
            string orientation
        )
        {
            Orientation = orientation;
            Width = width;
            Height = height;
            TileWidth = tilewidth;
            TileHeight = tileheight;
            WidthInPixels = width * tilewidth;
            HeightInPixels = height * tileheight;

            this.Layers = Layers.OrderBy(l => l.Depth).ToDictionary(l => l.Name, l => l);
            this.TileSets = TileSets.ToDictionary(tSet => tSet.ID, tSet => tSet);

            // update each layer tile with additional info
            foreach(var layer in Layers)
            {
                if(layer.Width != width || layer.Height != height)
                {
                    throw new Exception($"Layer ({layer.Name}) must have width({layer.Width}) and height({layer.Height}) equals map with ({width}) and map height({height})");
                }

                for(int i = 0; i < layer.Width; i++)
                {
                    for(int j = 0; j < layer.Height; j++)
                    {
                        if (layer.Data[i, j] == null)
                            continue;

                        var layerTile = layer.Data[i, j];
                        var tile = Tiles.SingleOrDefault(t => t.ID == layerTile.ID);
                        layerTile.Height = TileHeight;
                        layerTile.Width = TileWidth;
                        layerTile.TileOrder = tile != null ? tile.TileOrder : null;
                        layerTile.Tileset = tile != null ? tile.Tileset : null;
                        layerTile.Properties = tile != null ? tile.Properties : new Dictionary<string, EzmCustomProperty>();                     
                    }
                }
            }
        }

        public EzmMap(
            ContentManager content,
            GraphicsDevice graphicsDevice,
            string pathToTilesetsFolders, 
            string pathToEzmFile, 
            bool showTileBorder = false,
            Vector2? mapOrigin = null)
        {
            var map = EzmLoader.LoadMap(pathToEzmFile, pathToTilesetsFolders, content);

            this.Origin = mapOrigin.HasValue ? mapOrigin.Value : Vector2.Zero;
            this.Content = content;
            this.GraphicsDevice = graphicsDevice;
            this.Height = map.Height;
            this.HeightInPixels = map.HeightInPixels;
            this.Orientation = map.Orientation;
            this.TileHeight = map.TileHeight;
            this.TileWidth = map.TileWidth;
            this.Width = map.Width;
            this.WidthInPixels = map.WidthInPixels;
            this.Layers = map.Layers;
            this.TileSets = map.TileSets;
            this.ShowTileBorder = showTileBorder;
            this.pixel =  new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.pixel.SetData(new[] { Color.White });
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Orientation == "orthogonal")
                DrawOrthogonal(spriteBatch);
            else if (Orientation == "isometric")
                DrawIsometric(spriteBatch);
        }

        public void DrawOrthogonal(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            foreach (var l in Layers.Values.OrderBy(l => l.Depth))
            {
                for (int i = 0; i < l.Width; i++)
                {
                    for (int j = 0; j < l.Height; j++)
                    {
                        var tile = l.Data[i, j];

                        // empty tiles
                        if (tile == null)
                            continue;

                        var tilesetTexture = TileSets[tile.Tileset.Value].Texture;
                        var targetLocation = new Rectangle((tile.Column * tile.Width) + (int)Origin.X, (tile.Row * tile.Height) + (int)Origin.Y, tile.Width, tile.Height);

                        spriteBatch.Draw(tilesetTexture, targetLocation, tile.TileSetArea, tile.Color);

                        if(ShowTileBorder)
                            Utils.Utils.DrawTileBorder(spriteBatch, pixel, targetLocation, 1, Color.Red);
                    }
                }
            }

            spriteBatch.End();
        }

 

        public void DrawIsometric(SpriteBatch spriteBatch)
        {

        }

        public List<EzmTile> GetTilesIntersecsWith(Rectangle area)
        {
            var intersectList = new List<EzmTile>();
            foreach (var l in Layers.Values)
            {
                for (int i = 0; i < l.Width; i++)
                {
                    for (int j = 0; j < l.Height; j++)
                    {
                        var tile = l.Data[i, j];
                        if (tile == null)
                            continue;

                        var tileRect = new Rectangle(tile.Column * tile.Width, tile.Row * tile.Height, tile.Width, tile.Height);
                        
                        if (tileRect.Intersects(area))
                        {
                            intersectList.Add(tile);
                        }
                    }
                }
            }

            return intersectList;
        }

        public void Unload()
        {
            // Unload tileset textures
            foreach (var tSet in TileSets.Values)
            {
                TileSets[tSet.ID].Texture.Dispose();
            }
        }

        public void Dispose()
        {
            Unload();
        }
    }
}
