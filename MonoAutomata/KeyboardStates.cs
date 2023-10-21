using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MonoAutomata
{
    public class KeyboardStates
    {
        public Keys[] Down = new Keys[0];
        public Keys[] Last = new Keys[0];
        public Keys[] Pressed = new Keys[0];
        public Keys[] Up = new Keys[0];

        public bool KeyPressed(Keys key) => Pressed.Contains(key);
        public bool KeyDown(Keys key) => Down.Contains(key);
        public bool KeyUp(Keys key) => Down.Contains(key);

        public void UpdateState() 
        {
            var keyState = Keyboard.GetState();
            Down = keyState.GetPressedKeys();

            // all keys that weren't down last update but are now
            Pressed = Down.Except(Last).ToArray();

            // all keys that were down last update, but aren't anymore
            Up = Last.Except(Down).ToArray();

            Last = Down;
        }
    }
}
