using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzmLoader.Components
{
    //[JsonObject(MemberSerialization. OptIn)]
    public class EzmTileSet
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("tilecount")]
        public int TileCount { get; set; }

        [JsonProperty("columns")]
        public int Columns { get; set; }

        [JsonProperty("rows")]
        public int Rows { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("content")]
        private string Content { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("encoded")]
        public bool Encoded { get; set; }

        [JsonIgnore]
        public Texture2D Texture { get; set; }
    }
}
