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
        public EzmTile [,]Data2 { get; set; }

        public int[] Data { get; set; }


        [JsonConstructor]
        public EzmLayer(int width, int height, int []data)
        {
            this.Width = width;
            this.Height = height;
            this.Data = data; // remover depois
            Data2 = new EzmTile[width, height];
            for(int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var tID = data[(i + j * width)];
                    Data2[i,j] = new EzmTile()
                    {
                        ID = tID,
                        Color = Color.White,
                        Row = i,
                        Column = j
                    };
                }
            }            
        }
    }
}
