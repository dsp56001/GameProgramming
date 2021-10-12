using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManGameComponent
{
    //Make a ghost component that roams randomly on screen
    //It should pick a random direction to move when is starts and bounce off of wall when it collides with them
    
    public class Ghost : Sprite
    {



        public Ghost(Game game) : base(game)
        {

        }

        protected override void LoadContent()
        {
            this.SpriteTexture = this.Game.Content.Load<Texture2D>("")
            base.LoadContent();
        }
    }


}
