using System;

using JankWorks.Graphics;

namespace Pong.UI
{
    interface IControl : IElement
    {
        Bounds GetBounds();

        void Enter();

        void Leave();

        void Click();
    }
}