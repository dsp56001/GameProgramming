﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Sprite2;
using IntroPacManComponent;

namespace StrategyPacMan.weapons
{
    class TealWeapon : foodWeapon
    {
        public TealWeapon(Game game)
            : base(game)
        {
            this.Name = "red weapon";
            this.Verb = "rec chomp";
            this.color = Color.Teal;
        }
    }
}
