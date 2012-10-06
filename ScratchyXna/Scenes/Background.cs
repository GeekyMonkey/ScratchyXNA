using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ScratchyXna
{
    public class Background
    {
        /// <summary>
        /// Background Layers
        /// </summary>
        public List<BackgroundLayer> Layers = new List<BackgroundLayer>();
        internal Scene scene;
        int scrollControlLayer = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public Background(Scene scene)
        {
            Init(scene);
        }

        internal void Init(Scene scene)
        {
            this.scene = scene;
        }

        /// <summary>
        /// The layer that's controlling the scrolling
        /// </summary>
        public int ScrollControlLayer
        {
            get
            {
                return scrollControlLayer;
            }
            set
            {
                scrollControlLayer = value;
            }
        }

        /// <summary>
        /// Scale the background to fit the height of one of the background layers
        /// </summary>
        /// <param name="layer">1 based layer index</param>
        public void ScaleToScreenHeight(int layer)
        {
            float newScale = GetLayer(layer).ScaleToScreenHeight();
            Layers.ForEach(l => l.Scale = newScale);
        }

        /// <summary>
        /// Scale the background to fit the width of one of the background layers
        /// </summary>
        /// <param name="layer">1 based layer index</param>
        public void ScaleToScreenWidth(int layer)
        {
            float newScale = GetLayer(layer).ScaleToScreenWidth();
            Layers.ForEach(l => l.Scale = newScale);
        }

        /// <summary>                 
        /// Scale so that we completely fill the screen
        /// </summary>
        public void ScaleToScreen()
        {
            ScaleToScreen(1);
        }

        /// <summary>
        /// Scale so that we completely fill the screen
        /// </summary>
        /// <param name="layer">1 based layer index</param>
        public void ScaleToScreen(int layer)
        {
            BackgroundLayer backgroundLayer = GetLayer(layer);
            float heightScale = backgroundLayer.ScaleToScreenHeight();
            float widthScale = backgroundLayer.ScaleToScreenWidth();
            float scale = (widthScale < heightScale) ? heightScale : widthScale;
            Layers.ForEach(l => l.Scale = scale);
        }

        /// <summary>
        /// Update the background layer(s)
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        public void UpdateBackground(GameTime gameTime)
        {
            // todo : move parallax attached sprites
            Update(gameTime);
            foreach (BackgroundLayer layer in Layers)
            {
                layer.UpdateBackgroundLayer(gameTime);
            }
        }

        /// <summary>
        /// Updating the background layer(s)
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Add a background layer at depth 1
        /// </summary>
        /// <param name="backgroundImageName">The image resource name (in the Backgrounds folder)</param>
        /// <returns>New background layer</returns>
        public BackgroundLayer AddLayer(string backgroundImageName)
        {
            return AddLayer(backgroundImageName, 1);
        }

        /// <summary>
        /// Return a layer by index.  1 is the first one added. These are the 1-2-3 indexes, not the layer depth.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public BackgroundLayer GetLayer(int layerIndex)
        {
            return Layers[layerIndex - 1];
        }

        /// <summary>
        /// Add a background layer
        /// </summary>
        /// <param name="backgroundImageName">The image resource name (in the Backgrounds folder)</param>
        /// <param name="layer">Layer depth. 1 = background, 101 = above text</param>
        /// <returns>New background layer</returns>
        public BackgroundLayer AddLayer(string backgroundImageName, int layer)
        {
            BackgroundLayer newLayer;
            newLayer = this.scene.Game.LoadBackgroundLayer(scene, this, backgroundImageName);
            newLayer.Layer = layer;
            Layers.Add(newLayer);
            return newLayer;
        }

        public void SetScrollX(double x)
        {
            if (x > MaxScrollX)
            {
                x = MaxScrollX;
            }
            else if (x < MinScrollX)
            {
                x = MinScrollX;
            }
            Layers[ScrollControlLayer - 1].SetScrollX(x);
        }

        public float MaxScrollX
        {
            get
            {
                return Layers.Max(l => l.MaxScrollX);
            }
        }

        public float MinScrollX
        {
            get
            {
                return Layers.Min(l => l.MinScrollX);
            }
        }

        public float MaxScrollY
        {
            get
            {
                return Layers.Max(l => l.MaxScrollY);
            }
        }

        public float MinScrollY
        {
            get
            {
                return Layers.Min(l => l.MinScrollY);
            }
        }

        public float MaxX
        {
            get
            {
                return MaxScrollX + scene.Width / 2f;
            }
        }

        public float MinX
        {
            get
            {
                return MinScrollX - scene.Width / 2f;
            }
        }

        public float MaxY
        {
            get
            {
                return MaxScrollY + scene.Height / 2f;
            }
        }

        public float MinY
        {
            get
            {
                return MaxScrollY - scene.Height / 2f;
            }
        }


    }
}
