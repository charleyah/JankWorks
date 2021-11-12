using JankWorks.Interface;
using JankWorks.Game;

namespace Pong.Match.Players
{
    sealed class InputPlayer : Player, IInputListener
    {
        public Key UpKey { get; set; }

        public Key DownKey { get; set; }

        private sbyte movement; // positive is up, negative is down

        public InputPlayer(byte number) : base(number) 
        {
            this.movement = 0;
            this.UpKey = Key.W;
            this.DownKey = Key.S;
        }

        public void SubscribeInputs(IInputManager inputManager)
        {            
            inputManager.OnKeyPressed += this.OnKeyPress;
            inputManager.OnKeyReleased += this.OnKeyRelease;
        }

        public void UnsubscribeInputs(IInputManager inputManager)
        {
            inputManager.OnKeyPressed -= this.OnKeyPress;
            inputManager.OnKeyReleased -= this.OnKeyRelease;
        }

        private void OnKeyPress(KeyEvent e)
        {
            if(!e.Repeated)
            {
                if(e.Key == this.UpKey)
                {
                    this.movement++;                    
                }
                else if (e.Key == this.DownKey)
                {
                    this.movement--;                    
                }
                this.SubmitMovement((PlayerMovement)this.movement);
            }
        }

        private void OnKeyRelease(KeyEvent e)
        {
            if (e.Key == this.UpKey)
            {
                this.movement--;
            }
            else if (e.Key == this.DownKey)
            {
                this.movement++;
            }
            this.SubmitMovement((PlayerMovement)this.movement);
        }
    }
}