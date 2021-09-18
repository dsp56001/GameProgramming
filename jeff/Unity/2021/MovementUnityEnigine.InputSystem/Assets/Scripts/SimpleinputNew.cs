using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    class SimpleinputNew : SimpleInput
    {
        protected override void UpdateInputFromKeyboard()
        {
            //base.UpdateInputFromKeyboard();

            keyDirection.x = keyDirection.y = 0;  //reset key direction on every frame this will stop movement if no ket is pressed

            var keyboard = Keyboard.current;
            if(keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed) //Carefull IsPressed() is a method
            {
                keyDirection.x += -1;
            }
            if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
            {
                keyDirection.x += 1;
            }
            if (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed)
            {
                keyDirection.y += -1;
            }
            if (keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed)
            {
                keyDirection.y += 1;
            }

            inputDirection = keyDirection;
            inputDirection.Normalize();

        }
    }
}
