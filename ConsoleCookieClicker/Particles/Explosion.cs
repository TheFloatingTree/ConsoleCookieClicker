using ConsoleCookieClicker.Util;
using System;
using System.Numerics;

namespace ConsoleCookieClicker.Particles
{
    public class Explosion : ParticleSystem
    {
        private Random rand = new Random();

        public Explosion() : this(Vector2.Zero) { }
        public Explosion(Vector2 position)
        {
            this.position = position;

            for (int i = 0; i < 3; i++)
            {
                particles.Add(GenerateParticle());
            }
        }

        public override bool Update()
        {
            // Iterate over all particles
            for (int i = 0; i < particles.Count; i++)
            {
                // Update and remove if dead
                if (particles[i].Update())
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
            // If there are no more particles
            return particles.Count <= 0;
        }

        public override void Draw()
        {
            for (int i = 0; i < this.particles.Count; i++)
            {
                ConsoleGraphics.FilledCircle((int)this.particles[i].position.X, (int)this.particles[i].position.Y, 3, ' ', '@');
            }
        }

        protected override Particle GenerateParticle()
        {
            // Random velocity
            float randomX = 2f * ((float)rand.NextDouble() - 0.5f);
            float randomY = 2f * ((float)rand.NextDouble() - 0.5f);

            // Construct new particle
            return new Particle(
                new Vector2(position.X, position.Y),
                new Vector2(randomX, randomY),
                new Vector2(0, 0.04f),
                1000
                );
        }
    }
}
