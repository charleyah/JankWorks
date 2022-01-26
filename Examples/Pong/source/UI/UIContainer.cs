using System;
using System.Numerics;
using System.Collections.Generic;

using JankWorks.Graphics;
using JankWorks.Interface;

using JankWorks.Game;
using JankWorks.Game.Assets;


namespace Pong.UI
{
    class UIContainer : IInputListener, IRenderable, IUpdatable
    {
        public OrthoCamera Camera { get; set; }

        public bool Interactive { get; set; }

        private uint fontSize;
        private uint titleFontSize;

        private TextRenderer titleRenderer;
        private TextRenderer textRenderer;
        private ShapeRenderer shapeRenderer;
       
        private List<IElement> elements;
        private List<IControl> controls;

        private IControl hoveredControl;
        private bool active;
        
        public UIContainer(uint fontSize, uint titleFontSize)
        {
            this.fontSize = fontSize;
            this.titleFontSize = titleFontSize;
            this.elements = new List<IElement>();
            this.controls = new List<IControl>();
            this.active = false;
        }

        public void AddElement(IElement element) => this.elements.Add(element);

        public void AddControl(IControl control)
        {
            this.elements.Add(control);
            this.controls.Add(control);
        }
        
        public void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets)
        {
            using var ttffont = assets.GetAsset("embedded", "ibm-plex-mono.regular.ttf");
            using var font = Font.LoadFromStream(ttffont, FontFormat.TrueType);

            font.FontSize = this.titleFontSize;
            this.titleRenderer = device.CreateTextRenderer(this.Camera, font);

            font.FontSize = this.fontSize;
            this.textRenderer = device.CreateTextRenderer(this.Camera, font);
            this.shapeRenderer = device.CreateShapeRenderer(this.Camera);
        }

        public void Activate()
        {
            this.elements.Sort((l, r) => l.ZOrder.CompareTo(r.ZOrder));
            this.controls.Sort((l, r) => l.ZOrder.CompareTo(r.ZOrder));
            this.active = true;
            this.Interactive = true;
        }

        public void Deactivate() => this.active = false;
        
        public void SubscribeInputs(IInputManager inputManager)
        {
            inputManager.OnMouseMoved += OnMouseMoved;
            inputManager.OnMouseButtonReleased += OnMouseButtonReleased;
        }

        public void UnsubscribeInputs(IInputManager inputManager)
        {
            inputManager.OnMouseMoved -= OnMouseMoved;
            inputManager.OnMouseButtonReleased -= OnMouseButtonReleased;
        }

        private void OnMouseMoved(Vector2 pos)
        {
            if(!this.active || !this.Interactive)
            {
                return;
            }

            var translated = this.Camera.TranslateScreenCoordinate(pos);
            pos = new Vector2(translated.X, translated.Y);

            var controlFound = false;

            for(int i = 0; i < this.controls.Count; i++)
            {
                var control = this.controls[i];

                if(control.GetBounds().Contains(pos))
                {
                    controlFound = true;

                    if (!object.ReferenceEquals(control, hoveredControl))
                    {
                        hoveredControl?.Leave();
                        this.hoveredControl = control;
                        this.hoveredControl.Enter();
                    }

                    break;
                }
            }

            if(!controlFound)
            {
                hoveredControl?.Leave();
                this.hoveredControl = null;
            }
        }

        private void OnMouseButtonReleased(MouseButtonEvent mbe)
        {
            if (!this.active || !this.Interactive)
            {
                return;
            }

            if(mbe.Button == MouseButton.Left)
            {
                this.hoveredControl?.Click();
            }            
        }

        public void Update(GameTime time)
        {
            for (int i = 0; i < this.elements.Count; i++)
            {
                this.elements[i].Update(time);
            }
        }

        public void Render(Surface surface, GameTime time)
        {
            if (!this.active)
            {
                return;
            }

            if(this.elements.Count > 0)
            {
                int currentZ = this.elements[0].ZOrder;

                this.shapeRenderer.BeginDraw();
                this.titleRenderer.BeginDraw();
                this.textRenderer.BeginDraw();

                for (int i = 0; i < this.elements.Count; i++)
                {
                    var element = this.elements[i];

                    if(currentZ != element.ZOrder)
                    {
                        this.shapeRenderer.EndDraw(surface);
                        this.titleRenderer.EndDraw(surface);
                        this.textRenderer.EndDraw(surface);

                        this.shapeRenderer.BeginDraw();
                        this.titleRenderer.BeginDraw();
                        this.textRenderer.BeginDraw();
                        currentZ = element.ZOrder;
                    }

                    element.Draw((element is Text text && text.IsTitle) ? this.titleRenderer : this.textRenderer, this.shapeRenderer);
                }

                this.shapeRenderer.EndDraw(surface);
                this.titleRenderer.EndDraw(surface);
                this.textRenderer.EndDraw(surface);               
            }
        }

        public void DisposeGraphicsResources(GraphicsDevice device)
        {
            this.shapeRenderer.Dispose();
            this.textRenderer.Dispose();
            this.titleRenderer.Dispose();
        }
    }
}