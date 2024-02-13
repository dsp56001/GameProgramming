using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace SimpleMovementWGravity
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Texture2D PacMan;
        //Vector2 PacManLoc;      //Pacman location
        //Vector2 PacManDir;      //Pacman direction
        //float PacManSpeed; //speed for the PacMan Sprite in pixels per frame per second

        ////Gravity
        ////float PacManSpeedMax;           //Max speed for the pac man sprite
        //Vector2 GravityDir;             //Gravity Direction normalized victor
        //float GravityAccel;             //Gavity Acceloration

        List<Pacman> pacmanPlayers;
        public int PLAYER_COUNT { get; private set; }

        SpriteFont font;

        public Game1(int playerCount)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            PLAYER_COUNT = playerCount;

            //graphics.PreferredBackBufferHeight = 720;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.ToggleFullScreen();  //changes to fullscreen

            //Change the framerate of the game to 30 frames per second
            //This is used to show how time changes animation speed or better yet that is shouldn't
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //PacMan = Content.Load<Texture2D>("pacManSingle");
            //Set PacMan Location to center of screen
            //PacManLoc = new Vector2(GraphicsDevice.Viewport.Width / 2,
            //    GraphicsDevice.Viewport.Height / 2);
            ////Vector for pacman direction
            ////notice this vector has no magnitude it's noramlized
            //PacManDir = new Vector2(1, 0);

            ////Pacman spped 
            //PacManSpeed = 10;
            ////PacManSpeedMax = 20;

            //GravityDir = new Vector2(1, 0);
            //GravityAccel = 1.8f;

            //pacmanPlayers = Enumerable.Repeat(new Pacman(this, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), 
            //    new Vector2(1, 0), 10, new Vector2(1, 0), 1.8f), PLAYER_COUNT).ToList();

            pacmanPlayers = new List<Pacman>()
            {
                new Pacman(this, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), new Vector2(0, 1), 10, new Vector2(0, 1), 1.8f),
                new Pacman(this, new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 4), new Vector2(0, 1), 10, new Vector2(0, 1), 1.8f)
            };

            font = Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //KeyboardState state = Keyboard.GetState();

            //Elapsed time since last update will be used to correct movement speed
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Move PacMan
            //Simple move Moves PacMac by PacManDiv on every update
            //PacManLoc = PacManLoc + PacManDir * PacManSpeed;

            for(int i = 0; i < pacmanPlayers.Count; i++)
            {
                pacmanPlayers[i].GravityMove(gameTime);
                pacmanPlayers[i].UpatePacmanKeepOnScreen();
                //Don't stop if no keys
                //pacmanPlayers[i].UpdatePacmanSpeed();
                pacmanPlayers[i].UpdateKeyboardInput();
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

            spriteBatch.Begin();
            for (int i = 0; i < pacmanPlayers.Count; i++)
            {
                spriteBatch.Draw(pacmanPlayers[i].PacManTxt, pacmanPlayers[i].PacManLoc, Color.White);
                spriteBatch.DrawString(font,
                    string.Format("Speed:{0}\nDir:{1}\nGravityDir:{2}\nGravtyAccel:{3}",
                    pacmanPlayers[i].PacManSpeed, pacmanPlayers[i].PacManDir, pacmanPlayers[i].GravityDir, pacmanPlayers[i].GravityAccel),
                    new Vector2(10, 10 * (i * 9.5f)),
                    Color.White);
                
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
