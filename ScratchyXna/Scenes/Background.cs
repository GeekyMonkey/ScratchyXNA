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
        private Scene scene;
        private float scale = 1.0f;

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
        /// Scale of the background
        /// </summary>
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        /// <summary>
        /// Scale the background to fit the height of one of the background layers
        /// </summary>
        public void ScaleToScreenHeight(int layer)
        {
            this.Scale = 200f / Layers[layer - 1].Height;
        }

        /// <summary>
        /// Scale the background to fit the width of one of the background layers
        /// </summary>
        public void ScaleToScreenWidth(int layer)
        {
            this.Scale = (this.scene.MaxX - this.scene.MinX) / Layers[layer - 1].Width;
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
        public void ScaleToScreen(int layer)
        {
            ScaleToScreenHeight(layer);
            float heightScale = Scale;
            ScaleToScreenWidth(layer);
            float widthScale = Scale;
            if (widthScale < heightScale)
            {
                Scale = heightScale;
            }
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
    }
}
