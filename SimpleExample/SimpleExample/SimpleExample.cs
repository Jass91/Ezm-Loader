using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shared.Enums;
using SimpleExample.Core.Player;
using SimpleExample.Core.Map;

namespace SimpleExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SimpleExample : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Map map;

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
            map = new Map(Content, $@"{Content.RootDirectory}\Levels\map.ezm", "TileSets");
            player = new Player(0);

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

            var t = map.GetTileAt(player.Position);
            var property = t.Properties.Count > 0 ? t.Properties["collidable"].Value : null;
            if (property != null)
            {
                t.Color = bool.Parse(property) ? Color.Red : Color.White;
            }else
            {
                map.Tiles[38].Color = Color.White;
            }

            player.Move();

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

            // TODO: Add your drawing code here
            map.Draw(spriteBatch);

            player.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
