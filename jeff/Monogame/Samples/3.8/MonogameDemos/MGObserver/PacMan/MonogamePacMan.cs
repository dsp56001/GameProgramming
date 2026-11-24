using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.GameComponents.Player;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGObserver
{
    public class MonogamePacMan : DrawableSprite
    {
        public IPlayerController Controller { get; protected set; }
        internal GameConsolePacMan PacMan
        {
            get;
            private set;
        }
        protected PacManState pacState;
        public PacManState PacState
        {
            get { return this.pacState; }
            //Change pacman state also
            set
            {
                if (this.pacState != value)
                {
                    this.pacState = this.PacMan.State = value; //change PacMan state that is encasulated
                    this.pacStateChanged();
                }
            }
        }

        /// <summary>
        /// Hook to allow child classes to recieve state change calls
        /// </summary>
        protected virtual void pacStateChanged()
        {
            //nothing yet
        }

        public MonogamePacMan(Game game)
            : base(game)
        {
            SetupIPlayerController(game);
            PacMan = new GameConsolePacMan((GameConsole)game.Services.GetService<IGameConsole>());
        }

        protected virtual void SetupIPlayerController(Game game)
        {
            this.Controller = new PlayerController(game);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.SpriteTexture = this.Game.Content.Load<Texture2D>("pacManSingle");
            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
            this.Location = new Microsoft.Xna.Framework.Vector2(100, 100);
            this.Speed = 200;
            this.pacState = PacManState.Spawning;
        }

        public override void Update(GameTime gameTime)
        {
            //Elapsed time since last update
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            switch(this.PacState)
            {
                case PacManState.Spawning:
                    //Not ready for spawing yet switch to still
                    UpdateSpawingTimer(gameTime);
                    break;
                case PacManState.Dying:
                    //this.Die();
                    break;
                case PacManState.SuperPacMan:
                    UpdatePacManWithController(gameTime, time);
                    UpdatePowerUpTimer(gameTime);
                    break;
                case PacManState.EndSuperPacMan:
                    this.PacState = PacManState.Chomping;
                    break;
                case PacManState.Chomping:
                case PacManState.Still:
                    UpdatePacManWithController(gameTime, time);
                    break;
                default:
                    break;
            }
            UpdateKeepPacManOnScreen();
            base.Update(gameTime);
        }

        float _spawningTimer = 2000;
        private void UpdateSpawingTimer(GameTime gameTime)
        {
            _spawningTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (_spawningTimer <= 0)
            {
                _spawningTimer = 5000; // Reset timer for next use
                this.PacState = PacManState.Still;
            }
        }

        float _powerUpTimer = 5000;
        private void UpdatePowerUpTimer(GameTime gameTime)
        { 
            _powerUpTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (_powerUpTimer <= 0)
            {
                _powerUpTimer = 5000; // Reset timer for next use
                this.EndPowerUp();
            }  
        }

        public virtual void Die()
        {
            this.PacMan.Die();
        }

        protected virtual void UpdatePacManWithController(GameTime gameTime, float time)
        {
            this.Controller.Update(gameTime);

            this.Location += ((this.Controller.Direction * (time / 1000)) * Speed);      //Simple Move 
            this.Rotate = this.Controller.Rotate;

            if (this.pacState != PacManState.SuperPacMan)
            {
                //Change State based on movement
                if (this.Controller.HasInputForMoverment)
                { 
                    this.PacState = PacManState.Chomping;
                }
                else
                { 
                    this.PacState = PacManState.Still;
                }
            }
        }

        protected void UpdateKeepPacManOnScreen()
        {
            //Keep PacMan On Screen
            if (this.Location.X > Game.GraphicsDevice.Viewport.Width - (this.spriteTexture.Width / 2))
            {
                this.Location.X = Game.GraphicsDevice.Viewport.Width - (this.spriteTexture.Width / 2);
            }
            if (this.Location.X < (this.spriteTexture.Width / 2))
                this.Location.X = (this.spriteTexture.Width / 2);

            if (this.Location.Y > Game.GraphicsDevice.Viewport.Height - (this.spriteTexture.Height / 2))
                this.Location.Y = Game.GraphicsDevice.Viewport.Height - (this.spriteTexture.Height / 2);

            if (this.Location.Y < (this.spriteTexture.Height / 2))
                this.Location.Y = (this.spriteTexture.Height / 2);
        }

        public virtual void PowerUp()
        {
            
                this.PacState = PacManState.SuperPacMan;
        }

        public virtual void EndPowerUp()
        {
            this.PacState = PacManState.EndSuperPacMan;
        }
    }
}
