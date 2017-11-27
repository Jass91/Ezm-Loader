using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public int? TileOrder { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("tileset")]
        public int? Tileset { get; set; }

        // talvez remover -------------
        [JsonIgnore]
        public int TileRow { get; set; }

        [JsonIgnore]
        public int TileCol { get; set; }
        // ------------------------------

        [JsonIgnore]
        public Dictionary<string, EzmCustomProperty> Properties { get; set; }

        [JsonIgnore]
        public Color Color { get; set; } = Color.White;

        [JsonIgnore]
        public int Column { get; set; }

        [JsonIgnore]
        public int Row { get; set; }

        [JsonIgnore]
        public Rectangle TileArea { get; set; }

        [JsonConstructor]
        private EzmTile(ICollection<EzmCustomProperty> properties)
        {
            this.Properties = properties.ToDictionary(p => p.Name , p  => p);
        }
        
        public EzmTile()
        {

        }      
    }
}