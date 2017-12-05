using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleExample.Core.Player;
using EzmLoader;

namespace SimpleExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SimpleExample : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        EzmMap map;

        int fps = 15;
        int increase = 0;

        public SimpleExample()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: Add your initialization logic here
            map = new EzmMap(
                Content,
                GraphicsDevice,
                "TileSets",
                $@"{Content.RootDirectory}\Levels\map.ezm", 
                true
            );

            player = new Player(0, GraphicsDevice);

            setResolution();

            base.Initialize();
        }

        private void setResolution(int width = 800, int height = 600, bool fullscreen = false)
        {
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            graphics.IsFullScreen = fullscreen;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            player.LoadTexture(Content.Load<Texture2D>(@"Players\players"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            map.Unload();
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();
            
            UpdatePlayerDirection();
            var nexPosition = player.GetNextPostion();
            if (!PlayerWillCollide(new Rectangle((int)nexPosition.X, (int)nexPosition.Y, player.Width, player.Height)))
            {
                player.MoveTo(nexPosition);
            }

            if (PlayerWillEntryOnCavern(new Rectangle((int)nexPosition.X, (int)nexPosition.Y, player.Width, player.Height)))
            {
                map = new EzmMap(
                Content,
                GraphicsDevice,
                "TileSets",
                $@"{Content.RootDirectory}\Levels\cavern.ezm",
                true
            );
            }

            increase += gameTime.ElapsedGameTime.Milliseconds;
            if (increase >= 1000 / fps)
            {
                player.Animate();
                increase = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Somewhere in your LoadContent() method:
            var pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it

            map.Draw(spriteBatch);

            player.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        protected bool PlayerWillEntryOnCavern(Rectangle playerArea)
        {
            // this is more easy, we just need to use map.GetTilesIntersecsWith to get the tiles the intersects with playerArea
            foreach (var tile in map.GetTilesIntersecsWith(playerArea))
            {
                if(tile.Properties.ContainsKey("cavern"))
                    return true;
            }

            return false;
        }

        protected bool PlayerWillCollide(Rectangle playerArea)
        {
            // this is more easy, we just need to use map.GetTilesIntersecsWith to get the tiles the intersects with playerArea
            foreach (var tile in map.GetTilesIntersecsWith(playerArea))
            {
                var collidable = tile.Properties.ContainsKey("collidable") ? bool.Parse(tile.Properties["collidable"].Value) : false;

                if (collidable)
                    return true;
            }

            return false;

            // this is a bit faster, cuz you dont need to iterate over result again
            //foreach(var l in map.Layers.Values)
            //{
            //    for(int i = 0; i < l.Width; i++)
            //    {
            //        for (int j = 0; j < l.Height; j++)
            //        {
            //            var tile = l.Data[i, j];

            //            var tileRect = new Rectangle(tile.Column * tile.Width, tile.Row * tile.Height, tile.Width, tile.Height);
            //            var collidable = tile.Properties.ContainsKey("collidable") ? bool.Parse(tile.Properties["collidable"].Value) : false;

            //            if (tileRect.Intersects(playerArea) && collidable)
            //                return true;
            //        }
            //    }
            //}

            //return false;
        }

        protected void UpdatePlayerDirection()
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
            {
                player.Direction = Shared.Enums.Direction.UP;
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
            {
                player.Direction = Shared.Enums.Direction.LEFT;
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S))
            {
                player.Direction = Shared.Enums.Direction.DOWN;
            }
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
            {
                player.Direction = Shared.Enums.Direction.RIGHT;
            }
        }
    }
}
