
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace IntroGameLibrary.Util
{
    /// <summary>
    /// This is a game component that implements DrawableGameComponent.
    /// It's to profile performance
    /// </summary>
     

    public sealed class FPS : Microsoft.Xna.Framework.DrawableGameComponent
    {
        

        private bool updateTimeFixed;
        private bool synchronizeWithVerticalRetrace;

        float frameRate = 0;
        float frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        string fps;

        public FPS(Game game, bool synchWithVerticalRetrace, bool isFixedTimeStep)
            : this(game, synchWithVerticalRetrace, isFixedTimeStep,
                   game.TargetElapsedTime) { }
        
        public FPS(Game game) : this(game, false, true) { }

        public FPS(Game game, bool synchWithVerticalRetrace,
                   bool isFixedTimeStep, TimeSpan targetElapsedTime)
            : base(game)
        {
            GraphicsDeviceManager graphics =
                (GraphicsDeviceManager)Game.Services.GetService(
                typeof(IGraphicsDeviceManager));

            graphics.SynchronizeWithVerticalRetrace = synchWithVerticalRetrace;
            Game.IsFixedTimeStep = isFixedTimeStep;
            Game.TargetElapsedTime = targetElapsedTime;

            updateTimeFixed = Game.IsFixedTimeStep;
            graphics.ApplyChanges(); 
        }

        public void ToggleTimeFixed()
        {
            if (updateTimeFixed)
            {
                updateTimeFixed = false;
                
            }
            else
            {
                updateTimeFixed = true;

            }
            GraphicsDeviceManager graphics =
            (GraphicsDeviceManager)Game.Services.GetService(
            typeof(IGraphicsDeviceManager));

            Game.IsFixedTimeStep = updateTimeFixed;
            graphics.ApplyChanges(); 
        }

        public void ToggleSynchronizeWithVerticalRetrace()
        {
            if (synchronizeWithVerticalRetrace)
            {
                synchronizeWithVerticalRetrace = false;
            }
            else
            {
                synchronizeWithVerticalRetrace = true;
            }

            GraphicsDeviceManager graphics =
                (GraphicsDeviceManager)Game.Services.GetService(
                typeof(IGraphicsDeviceManager));

            graphics.SynchronizeWithVerticalRetrace = synchronizeWithVerticalRetrace;
            graphics.ApplyChanges(); 
        }

        /// <summary>
        /// Allows the game component to perform any initialization 
        /// it needs to before starting to run.  This is where it can query for
        /// any required services and load content.
        /// </summary>
        public sealed override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides snapshot of timing values.</param>
        public sealed override void Update(GameTime gameTime)
        {
#if DEBUG
            // TODO: Add your update code here
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

#endif
#if WINDOWS
            Game.Window.Title = "FPS: " + fps + " " + gameTime.IsRunningSlowly;
#endif
            base.Update(gameTime);
        }

        public sealed override void Draw(GameTime gameTime)
        {
#if DEBUG
            frameCounter++;
            fps = string.Format("fps: {0}", frameRate);
#if XBOX360
                System.Diagnostics.Debug.WriteLine("FPS: " + fps);
#else
            //int fps = (int)Math.Round(1.0 / gameTime.ElapsedGameTime.TotalSeconds);    
            //Game.Window.Title = "FPS: " + fps + " " + gameTime.IsRunningSlowly;
#endif   
#endif
            base.Draw(gameTime);
        }

        
    }
}
