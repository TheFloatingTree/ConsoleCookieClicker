using System.Collections.Generic;
using System.Numerics;

namespace ConsoleCookieClicker.Util
{
    static class ParticleManager
    {
        static private List<ParticleSystem> systems = new List<ParticleSystem>();

        static public void Add(ParticleSystem ps)
        {
            systems.Add(ps);
        }

        static public void Run()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                // Update and Draw, remove if finished.
                if (systems[i].Update())
                {
                    systems.RemoveAt(i);
                    i--;
                }
                else
                {
                    systems[i].Draw();
                }
            }
        }

    }

    public abstract class ParticleSystem
    {
        protected List<Particle> particles = new List<Particle>();
        protected Vector2 position = new Vector2();

        protected abstract Particle GenerateParticle();
        public abstract bool Update();

        // Default particle draw behavior
        public virtual void Draw()
        {
            for (int i = 0; i < this.particles.Count; i++)
            {
                ConsoleGraphics.Pixel((int)this.particles[i].position.X, (int)this.particles[i].position.Y);
            }
        }
    }

    public class Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public int life;

        public Particle() : this(Vector2.Zero, Vector2.Zero, Vector2.Zero, 0) { }

        public Particle(Vector2 position, Vector2 velocity, Vector2 acceleration, int life)
        {
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            if (life < 0)
                this.life = 0;
            else
                this.life = life;
        }

        public bool Update()
        {
            velocity.X += acceleration.X * (float) DeltaTimer.Get();
            velocity.Y += acceleration.Y * (float)DeltaTimer.Get();
            position.X += velocity.X * (float) DeltaTimer.Get();
            position.Y += velocity.Y * (float) DeltaTimer.Get();

            life--;
            return life <= 0;
        }
    }
}