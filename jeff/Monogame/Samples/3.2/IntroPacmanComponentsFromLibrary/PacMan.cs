using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;

namespace IntroPacManComponentsFromLibrary
{
    class PacMan : DrawableSprite
    {
        int pi; //player index for controller

        //Services
        InputHandler input;
        GameConsole console;
        ScoreService score;
        
        public PacMan(Game game)
            : base(game)
        {
            // TODO: Construct any child components her
            pi = 0;

            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));

            //Make sure input service exists
            if (input == null)
            {
                throw new Exception("PacMan Depends on Input service please add input service before you add PacMan.");
            }

            console = (GameConsole)game.Services.GetService(typeof(IGameConsole));
            //Make sure input service exists
            if (console == null)
            {
                throw new Exception("PacMan Depends on Console service please add Console service before you add PacMan.");
            }

            score = (ScoreService)game.Services.GetService(typeof(IScoreService));
            //Make sure input service exists
            if (console == null)
            {
                throw new Exception("PacMan Depends on Score service please add Score service before you add PacMan.");
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdatePacMan(lastUpdateTime, gameTime);
            
        }

        protected override void LoadContent()
        {
            this.spriteTexture = content.Load<Texture2D>("pacManSingle");
            
            this.Location = new Vector2(300, 300);
            this.Speed = 100;
            base.LoadContent();
            this.Orgin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
        }
        
        private void UpdatePacMan(float lastUpdateTime, GameTime gameTime)
        {
            //GamePad
            
            //Input for update from analog stick
            #region LeftStick
            if (input.GamePads[pi].ThumbSticks.Left.Length() > 0.0f)
            {

                Direction = input.GamePads[pi].ThumbSticks.Left;
                Direction.Y *= -1;      //Invert Y Axis

                float RotationAngle = (float)Math.Atan2(
                    input.GamePads[pi].ThumbSticks.Left.X,
                    input.GamePads[pi].ThumbSticks.Left.Y);

                Rotate = (float)MathHelper.ToDegrees(RotationAngle - (float)(Math.PI / 2));


                //Time corrected move. MOves PacMan By PacManDiv every Second
                Location += ((Direction * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir

                //Keep PacMan On Screen
                if (Location.X > graphics.GraphicsDevice.Viewport.Width - (spriteTexture.Width / 2))
                    Location.X = graphics.GraphicsDevice.Viewport.Width - (spriteTexture.Width / 2);

                if (Location.X < (spriteTexture.Width / 2))
                    Location.X = (spriteTexture.Width / 2);
            }
            #endregion

            //Update for input from DPad
            #region DPad
            Vector2 PacManDDir = Vector2.Zero;
            if (input.GamePads[pi].DPad.Left == ButtonState.Pressed)
            {
                //Orginal Position is Right so flip X
                PacManDDir += new Vector2(-1, 0);
            }
            if (input.GamePads[pi].DPad.Right == ButtonState.Pressed)
            {
                //Original Position is Right
                PacManDDir += new Vector2(1, 0);
            }
            if (input.GamePads[pi].DPad.Up == ButtonState.Pressed)
            {
                //Up
                PacManDDir += new Vector2(0, -1);
            }
            if (input.GamePads[pi].DPad.Down == ButtonState.Pressed)
            {
                //Down
                PacManDDir += new Vector2(0, 1);
            }
            if (PacManDDir.Length() > 0)
            {

                //Angle in radians from vector
                float RotationAngleKey = (float)Math.Atan2(
                        PacManDDir.X,
                        PacManDDir.Y * -1);
                //Find angle in degrees
                Rotate = (float)MathHelper.ToDegrees(
                    RotationAngleKey - (float)(Math.PI / 2)); //rotated right already

                //move the pacman
                
                Location += ((Vector2.Normalize(PacManDDir) * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir
            }
            #endregion

            //Update for input from Keyboard
#if !XBOX360
            #region KeyBoard
           
            Vector2 PacManKeyDir = new Vector2(0, 0);

            if (input.KeyboardState.IsKeyDown(Keys.Left))
            {
                //Flip Horizontal
                if (input.KeyboardState.WasKeyPressed(Keys.Left))
                {
                    console.GameConsoleWrite("Pressed Left!" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                    
                }
                score.CurrentScore++;
                PacManKeyDir += new Vector2(-1, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                //No new sprite Effects
                if (input.KeyboardState.WasKeyPressed(Keys.Right))
                {
                    console.GameConsoleWrite("Pressed Right!" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                }
                PacManKeyDir += new Vector2(1, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Up))
            {

                PacManKeyDir += new Vector2(0, -1);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Down))
            {

                PacManKeyDir += new Vector2(0, 1);
            }
            if (PacManKeyDir.Length() > 0)
            {

                float RotationAngleKey = (float)Math.Atan2(
                        PacManKeyDir.X,
                        PacManKeyDir.Y * -1);

                Rotate = (float)MathHelper.ToDegrees(
                    RotationAngleKey - (float)(Math.PI / 2));


                Location += ((Vector2.Normalize(PacManKeyDir) * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir
            }
            #endregion
#endif

            console.DebugText = this.Location.ToString();

        }

    }
}
