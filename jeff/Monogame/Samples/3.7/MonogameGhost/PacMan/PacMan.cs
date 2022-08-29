using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public enum PacManState { Spawning, Still, Chomping, SuperPacMan, Dying, Dead }

    public class PacMan
    {

        protected PacManState _state;
        public PacManState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    this.Log(string.Format("{0} was: {1} now {2}", this.ToString(), _state, value));

                    _state = value;
                }
            }
        }

        public PacMan()
        {
            //Set default state will call notify so make sure this.Ghosts is intitialized first
            this.State = PacManState.Still;
        }

        public void PowerUp()
        {
            this.State = PacManState.SuperPacMan;
        }

        public void PowerDown()
        {
            this.State = PacManState.Still;
        }

        public void Move()
        {
            this.State = PacManState.Chomping;
        }

        public void Kill()
        {
            this.State = PacManState.Dying;
        }

        public void Spawn()
        {
            this.State = PacManState.Spawning;
        }

        //Extra method for logging state change
        public virtual void Log(string s)
        {
            //nothing
            Console.WriteLine(s);
        }
    }
}
