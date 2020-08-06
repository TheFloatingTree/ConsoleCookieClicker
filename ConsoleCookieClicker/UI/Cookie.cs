using ConsoleCookieClicker.Particles;
using ConsoleCookieClicker.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCookieClicker.UI
{
    public class Cookie
    {
        private bool isPressed = false;
        private bool shouldWait = false;

        private const int cookieRadius = 10;

        public delegate void OnCookiePressHandler();
        public event OnCookiePressHandler OnCookiePress;

        public void Update()
        {
            if (InputManager.GetKey(ConsoleKey.Spacebar))
            {
                // Logic for held inputs, repeats but not too fast
                if (shouldWait) return;
                if (isPressed)
                {
                    shouldWait = true;
                    Task.Delay(150).ContinueWith(task => shouldWait = false);
                    return;
                }

                // When spacebar is pressed do this
                isPressed = true;
                OnCookiePress?.Invoke();
                Task.Delay(80).ContinueWith(task => isPressed = false);
                ParticleManager.Add(new Explosion(new Vector2(
                    ConsoleGraphics.ConsoleHalfWidth, 
                    ConsoleGraphics.ConsoleHalfHeight
                    )));
            }
        }

        public void Draw()
        {
            int radius = isPressed ? cookieRadius - 1 : cookieRadius;
            ConsoleGraphics.FilledCircle(ConsoleGraphics.ConsoleHalfWidth, ConsoleGraphics.ConsoleHalfHeight, radius, ' ', '@');
        }
    }
}
