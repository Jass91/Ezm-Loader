using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EzmLoader.Components
{
    public class EzmTile
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("tileorder")]
        public int TileOrder { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("tileset")]
        public int Tileset { get; set; }

        [JsonIgnore]
        public int TileRow { get; set; }

        [JsonIgnore]
        public int TileCol { get; set; }

        [JsonIgnore]
        public Dictionary<string, EzmCustomProperty> Properties { get; set; }

        [JsonIgnore]
        public Color Color { get; set; } = Color.White;

        [JsonConstructor]
        private EzmTile(ICollection<EzmCustomProperty> Properties)
        {
            this.Properties = Properties.ToDictionary(p => p.Name , p  => p);
        }
                
    }
}