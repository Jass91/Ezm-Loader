using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzmLoader.Components
{
    public class EzmLayer
    {
        [JsonProperty("depth")]
        public int Depth { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonIgnore]
        public EzmTile [,]Data { get; set; }


        [JsonConstructor]
        public EzmLayer(int width, int height, int []data)
        {
            this.Width = width;
            this.Height = height;

            // converts to EzamTile[,]
            Data = new EzmTile[width, height];
            for(int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var tileID = data[(j + i * width)];
                    Data[i, j] = new EzmTile()
                    {
                        ID = tileID,
                        Color = Color.White,
                        Row = i,
                        Column = j
                    };
                }
            }            
        }
    }
}
