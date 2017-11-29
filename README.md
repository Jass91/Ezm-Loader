# Ezm-Loader
A new map format and XNA parse to easily describe and render 2D maps.

You just need this line to load a map:
```
var map = EzmLoader.EzmLoader.LoadMap(pathToEzmMap, PathToTileSetsFolder, content);
```

And just this line to render a map:
```
map.Draw(spriteBatch);
```

The ezm file looks like:
```
{	
	"width": 5,
	"height": 5,
	"tilewidth": 32,
	"tileheight": 32,
	"orientation": "orthogonal",
  "tilesets": [
    {
      "id": 1,
      "name": "Level1",
      "columns": 8,
      "rows": 29,
      "width": 256,
      "height": 928,
      "source": "../TileSets/Level1.png"
    }
  ],
	"tiles": [
	{
      "id": 38,
      "TileOrder": 38,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": [
				{
					"name" : "collidable",
					"value": "true"
				}
			]
		},
		{
      "id": 205,
      "TileOrder": 205,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 206,
      "TileOrder": 206,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 207,
      "TileOrder": 207,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 213,
      "TileOrder": 213,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 214,
      "TileOrder": 214,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 215,
      "TileOrder": 215,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 221,
      "TileOrder": 221,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 222,
      "TileOrder": 222,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 223,
      "TileOrder": 223,
      "tileset": 1,
      "width": 32,
      "height": 32,
			"properties": []
		},
		{
      "id": 22,
      "TileOrder": 22,
      "tileset": 1,
      "width": 32,
      "height": 32,
      "properties": []
		}
	],
	"layers": [
    {
      "name": "Ground",
      "depth": 0,
      "width": 5,
      "height": 5,
      "data": [
        205,
        206,
        206,
        206,
        207,
        213,
        214,
        214,
        214,
        215,
        213,
        214,
        214,
        214,
        215,
        213,
        214,
        214,
        214,
        215,
        221,
        222,
        222,
        222,
        223
      ]
    },
    {
      "name": "Rocks",
      "depth": 1,
      "width": 5,
      "height": 5,
      "data": [
        38,38,38,38,38,
        38,-1,-1,-1,38,
        38,-1,-1,-1,38,
        38,-1,-1,-1,38,
		    38,38,38,38,38
      ]
    }
	]	
}

```
