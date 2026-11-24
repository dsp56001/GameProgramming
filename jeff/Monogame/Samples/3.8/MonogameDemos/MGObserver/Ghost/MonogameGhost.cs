using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGObserver
{
    public class MonogameGhost : DrawableSprite
    {
        static int _ghostCount;
        protected GameConsoleGhost ghost;
        public GameConsoleGhost Ghost
        {
               get { return this.ghost; }
               protected set { this.ghost = value; }
        }

        protected MonogamePacMan pacMan;
        Vector2 _chaseLocation;

        public Texture2D ghostHit;
        public Texture2D ghostTexture;
        public string strGhostTexture;

        Random r;

        public MonogameGhost(Game game, MonogamePacMan pacMan)
            : base(game)
        {
            this.pacMan = pacMan;
            this.ghost = new GameConsoleGhost((GameConsole)game.Services.GetService<IGameConsole>());
            
            strGhostTexture = "RedGhost";
            this.ghost.State = GhostState.Roving;
            
            r = new Random();
            _ghostCount++;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.ShowMarkers = false;
            this.Origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
        }

        protected override void LoadContent()
        {

            this.ghostTexture = this.Game.Content.Load<Texture2D>(strGhostTexture);
            this.ghostHit = this.Game.Content.Load<Texture2D>("GhostHit");
            this.spriteTexture = ghostTexture;
            this.Direction = new Vector2(0, 1);

            this.Speed = 100.0f;

            base.LoadContent();
        }
        float turnAmount;
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            turnAmount = .04f;


            switch (this.ghost.State)
            {
                case GhostState.Dead:

                    UpdateGhostDead();

                    break;
                case GhostState.Chasing:
                    UpdateGhostChase();
                    break;
                case GhostState.Evading:
                    UpdateGhostEvade();
                    break;
                case GhostState.Roving:
                    UpdateGhostRove();

                    break;
            }

            UpdateGhostKeepOnScreen();
            UpdateGhostCollision(gameTime);
            UpdateGhostTexture();

            Location += ((this.Direction * (lastUpdateTime / 1000)) * Speed);      //Simple Move


            base.Update(gameTime);
        }

        private void UpdateGhostTexture()
        {
            switch(this.ghost.State)
            {
                case GhostState.Evading:
                case GhostState.Dead:
                    this.spriteTexture = this.ghostHit;
                    break;
                case GhostState.Roving:
                case GhostState.Chasing:
                    this.spriteTexture = this.ghostTexture;
                    break;
            }
        }

        private void UpdateGhostCollision(GameTime gameTime)
        {
            //Collision
            if (this.Intersects(pacMan))
            {
                this.ghost.Log(this.ToString() + " Intersects PacsMan" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                //PerPixel Collision
                if (this.PerPixelCollision(pacMan))
                {
                    this.ghost.Log(this.ToString() + " Pixels touched PacsMan" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                    this.ghost.State = GhostState.Dead;
                }
            }
        }

        private void UpdateGhostKeepOnScreen()
        {
            
            //Borders Keep Ghost on the Screen
            if ((this.Location.Y + this.spriteTexture.Height / 2 > this.Game.GraphicsDevice.Viewport.Height)
                ||
                (this.Location.Y - this.spriteTexture.Height / 2 < 0)
                )
            {
                this.Direction.Y *= -1;
            }
            if ((this.Location.X + this.spriteTexture.Width / 2 > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X - this.spriteTexture.Width / 2 < 0)
                )
            {
                this.Direction.X *= -1;
            }
        }

        private void UpdateGhostRove()
        {   
            //check if ghost can see pacman
            Vector2 normD = Vector2.Normalize(this.Direction);
            Vector2 p = new Vector2(this.Location.X, this.Location.Y);
            while (p.X < this.Game.GraphicsDevice.Viewport.Width &&
                  p.X > 0 &&
                  p.Y < this.Game.GraphicsDevice.Viewport.Height &&
                  p.Y > 0)
            {
                if (pacMan.LocationRect.Contains(new Point((int)p.X, (int)p.Y)))
                {
                    this.ghost.State = GhostState.Chasing;
                    this.ghost.Log(this.ToString() + " saw pacman");
                    break;
                }
                p += this.Direction;
            }
        }

        private void UpdateGhostDead()
        {
            //TODO Dead moving and dead animation.
            //Until then
            this.ghost.State = GhostState.Roving;
            //Pick random direction
            this.Direction = this.GetRandomDirection(); ;                 //Assign random direction
        }

        private void UpdateGhostChase()
        { 
            if (pacMan.Location.Y > this.Location.Y)
            {
                //this.Direction.Y = 1;
                this.Direction.Y = MathHelper.Clamp(this.Direction.Y += turnAmount, -1, 1);
            }
            else
            {
                //this.Direction.Y = -1;
                this.Direction.Y = MathHelper.Clamp(this.Direction.Y -= turnAmount, -1, 1);
            }
            if (pacMan.Location.X > this.Location.X)
            {
                //this.Direction.X = 1;
                this.Direction.X = MathHelper.Clamp(this.Direction.X += turnAmount, -1, 1);
            }
            else
            {
                //this.Direction.X = -1;
                this.Direction.X = MathHelper.Clamp(this.Direction.X -= turnAmount, -1, 1);
            }
        }

        private void UpdateGhostEvade()
        { 
            if (pacMan.Location.Y > this.Location.Y)
            {
                this.Direction.Y = -1;
            }
            else
            {
                this.Direction.Y = 1;
            }
            if (pacMan.Location.X > this.Location.X)
            {
                this.Direction.X = -1;
            }
            else
            {
                this.Direction.X = -1;
            }
        }


        //Possible move to Sprite
        public Vector2 GetRandomDirection()
        {
            Vector2 v = new Vector2((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f);
            Vector2.Normalize(ref v, out v);    //Normalize
            return v;
        }

        public Vector2 GetRandLocation()
        {
            //System.Threading.Thread.Sleep(1);
            Vector2 loc;
            loc.X = r.Next(Game.Window.ClientBounds.Width - this.spriteTexture.Width) + this.spriteTexture.Width;
            loc.Y = r.Next(Game.Window.ClientBounds.Height - this.spriteTexture.Height) + this.spriteTexture.Height;
            return loc;
        }


    }
}
