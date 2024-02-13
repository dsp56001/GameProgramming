using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Microsoft.Xna.Framework.Input;

namespace SimpleMovementWGravity
{
    public class Pacman
    {
        public Texture2D PacManTxt { get; private set; }

        public Vector2 PacManLoc;      //Pacman location
        public Vector2 PacManDir;      //Pacman direction
        public float PacManSpeed; //speed for the PacMan Sprite in pixels per frame per second

        //Gravity
        //float PacManSpeedMax;           //Max speed for the pac man sprite
        public Vector2 GravityDir;             //Gravity Direction normalized victor
        public float GravityAccel;             //Gavity Acceloration
        Game1 Game1;
        public Pacman(Game1 game1, Vector2 pacManLoc, Vector2 pacManDir, float pacManSpeed, Vector2 gravityDir, float gravityAccel)
        {
            PacManTxt = game1.Content.Load<Texture2D>("pacManSingle");
            Game1 = game1;
            PacManLoc = pacManLoc;
            PacManDir = pacManDir;
            PacManSpeed = pacManSpeed;
            GravityDir = gravityDir;
            GravityAccel = gravityAccel;


        }

        private void Gravity()
        {
            //Gravity before move
            PacManDir = PacManDir + (GravityDir * GravityAccel);
        }

        public void Move(GameTime gameTime)
        {
            //Simple Move PacMan by PacManDir
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            PacManLoc = PacManLoc + ((PacManDir * PacManSpeed) * (time / 1000));
        }
        
        //Time corrected move. MOves PacMan By PacManDiv every Second
        public void GravityMove(GameTime gameTime) 
        {
            Gravity();
            Move(gameTime);
        }

        public void UpatePacmanKeepOnScreen()
        {
            //Keep PacMan On Screen
            //Turns around and stays at edges
            //X right
            if (PacManLoc.X >
                    Game1.GraphicsDevice.Viewport.Width - PacManTxt.Width)
            {
                //Negate X
                PacManDir = PacManDir * new Vector2(-1, 1);
                PacManLoc.X = Game1.GraphicsDevice.Viewport.Width - PacManTxt.Width;
            }

            //X left
            if (PacManLoc.X < 0)
            {
                //Negate X
                PacManDir = PacManDir * new Vector2(-1, 1);
                PacManLoc.X = 0;
            }

            //Y top
            if (PacManLoc.Y >
                    Game1.GraphicsDevice.Viewport.Height - PacManTxt.Height)
            {
                //Negate Y
                PacManDir = PacManDir * new Vector2(1, -1);
                PacManLoc.Y = Game1.GraphicsDevice.Viewport.Height - PacManTxt.Height;
            }

            //Y bottom
            if (PacManLoc.Y < 0)
            {
                //Negate Y
                PacManDir = PacManDir * new Vector2(1, -1);
                PacManLoc.Y = 0;
            }
        }

        public void UpdatePacmanSpeed()
        {
            //Speed for next frame
            if (Keyboard.GetState().GetPressedKeys().Length > 0) //If there is any key press the legth of the Array of keys returned by GetPressedKeys wil be greater that 0
            {
                PacManSpeed = 200;  //Key down pacman has speed
            }
            else
            {
                PacManSpeed = 0;    //No key down stop
            }

        }

        public void UpdateKeyboardInput()
        {
            #region Keyboard Movement
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                GravityDir = new Vector2(0, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                GravityDir = new Vector2(0, -1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                GravityDir = new Vector2(1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                GravityDir = new Vector2(-1, 0);
            }
            #endregion
        }
    }
}
