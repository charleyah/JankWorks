using System;
using System.Numerics;
using System.Collections.Generic;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Tests.RendererTest
{
    class RendererTest : Test
    {
        private OrthoCamera camera;
        private SpriteRenderer spriteRenderer;
        private Texture2D smiley;

        private Listener listener;
        private Sound scream;
        private Emitter screamer;

        private Smile? screamySmile;
        private Smile cursorSmile;
        private List<Smile> smiles;

        private Vector2i viewportSize;
        private Random rng;

        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.cursorSmile = new Smile()
            {
                colour = Colour.White
            };

            this.rng = new Random();
            this.smiles = new List<Smile>();

            this.viewportSize = graphics.Viewport.Size;

            this.listener = audio;
            this.listener.Orientation = Orientation.Ortho;
            this.scream = audio.LoadSound(GetEmbeddedStream("RendererTest.scream.wav"), AudioFormat.Wav);

            this.screamer = audio.CreateEmitter(this.scream);
            this.screamer.Loop = true;
            this.screamer.MaxDistance = 400;
            this.screamer.MinDistance = 100f;
            this.screamer.DistanceScale = 0.2f;
            this.screamer.Volume = 1f;

            this.camera = new OrthoCamera(this.viewportSize);

            this.spriteRenderer = graphics.CreateSpriteRenderer(this.camera, SpriteRenderer.DrawOrder.Reversed);
            
            this.smiley = graphics.CreateTexture2D(GetEmbeddedStream("RendererTest.smiley.png"), ImageFormat.PNG);

            graphics.DefaultDrawState = new DrawState()
            {
                Blend = BlendMode.Alpha,
                DepthTest = DepthTestMode.None
            };

            window.OnMouseButtonPressed += this.MousePressed;
            window.OnMouseMoved += this.MouseMoved;
        }

        private void MouseMoved(Vector2 screenPos)
        {
            var translated = this.camera.TranslateScreenCoordinate(screenPos);
            var pos = new Vector2(translated.X, translated.Y);

            ref var cursorSmile = ref this.cursorSmile;

            if(cursorSmile.position.X < pos.X)
            {
                cursorSmile.uv = new Bounds(1, 0, 0, 1);
            }
            else if (cursorSmile.position.X > pos.X)
            {
                cursorSmile.uv = Bounds.One;
            }

            cursorSmile.position = pos;
            this.listener.Position = new Vector3(pos, 0);
            this.spriteRenderer.Clear();
        }

        private void MousePressed(MouseButtonEvent e)
        {
            if(e.Button == MouseButton.Left)
            {
                RGBA shade = rng.Next(0, 4) switch
                {
                    0 => Colour.Blue,
                    1 => Colour.Pink,
                    2 => Colour.Cyan,
                    3 => Colour.White,
                    4 => Colour.Green,
                    _ => Colour.Yellow
                };

                Smile smile = this.cursorSmile;
                smile.colour = shade;

                this.smiles.Add(smile);                
            }
            else if(e.Button == MouseButton.Right)
            {
                if (this.screamySmile == null)
                {
                    this.screamySmile = new Smile()
                    {
                        position = new Vector2(-100, ((Vector2)this.viewportSize / 2).Y),
                        colour = Colour.Red,
                        uv = new Bounds(1, 0, 0, 1)
                    };
                    this.screamer.Position = new Vector3(this.screamySmile.Value.position, 0);
                    this.screamer.Play();
                }
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            if(!this.spriteRenderer.ReDraw(graphics))
            {
                this.spriteRenderer.BeginDraw();

                for (int index = 0; index < this.smiles.Count; index++)
                {
                    var smile = this.smiles[index];
                    this.spriteRenderer.Draw(this.smiley, smile.position, new Vector2(100), new Vector2(0.5f), 0, smile.colour, smile.uv);
                }

                var cursorSmile = this.cursorSmile;
                this.spriteRenderer.Draw(this.smiley, cursorSmile.position, new Vector2(100), new Vector2(0.5f), 0, cursorSmile.colour, cursorSmile.uv);

                if (this.screamySmile != null)
                {
                    var smile = this.screamySmile.Value;

                    this.spriteRenderer.Draw(this.smiley, smile.position, new Vector2(150), new Vector2(0.5f), 0, smile.colour, smile.uv);

                    smile.position += (Vector2.UnitX * 8);

                    this.screamer.Position = new Vector3(smile.position, 0);

                    if (smile.position.X > this.viewportSize.X + 100)
                    {
                        this.screamySmile = null;
                        this.screamer.Stop();
                    }
                    else
                    {
                        this.screamySmile = smile;
                    }
                }
                this.spriteRenderer.EndDraw(graphics);
            }          
        }

        public override void Dispose(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            window.OnMouseMoved -= this.MouseMoved;

            this.spriteRenderer.Dispose();
            this.smiley.Dispose();
            this.screamer.Dispose();
            this.scream.Dispose();
            graphics.DefaultDrawState = DrawState.Default;
            audio.Orientation = default;           
        }

        private struct Smile
        {
            public Vector2 position;
            public Bounds uv;
            public RGBA colour;            
        }
    }
}
