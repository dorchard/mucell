using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Drawing.Imaging;
using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;
using OpenTK;

namespace MuCell.View.OpenGL
{
    /// <summary>
    /// A call for managing OpenGL texture loaded, and associating textures
    /// with multiple OpenGL contexts
    /// </summary>
    public static class GLTextures
    {

        private static bool loaded = false;

        private static int[,] textures = new int[4,100] ;
        private static bool[,] textureSet = new bool[4,100];

        
        /// <summary>
        /// Setup OpenGL to correctly display textures
        /// </summary>
        public static void InitTextureSettings()
        {
            GL.Enable(EnableCap.Texture2d);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.Disable(EnableCap.Texture2d);
        }


        /// <summary>
        /// Loads a texture into graphs memory
        /// </summary>
        /// <param name="name">Texture path and name</param>
        /// <param name="index">Index of the texture for future references</param>
        /// <param name="view">The context into which to load the texture</param>
        private static void loadTex(String name, int index,int view)
        {

            if(!textureSet[view,index]){

            

                Bitmap bitmap = new Bitmap(name);
                GL.GenTextures(1, out textures[view,index]);
                GL.BindTexture(TextureTarget.Texture2d, textures[view,index]);

                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
          
                GL.TexImage2D(TextureTarget.Texture2d, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.OpenGL.Enums.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
           
                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
           

                textureSet[view,index] = true;
            }
        }


        /// <summary>
        /// Returns the name of a texture given its index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static String getTexName(int index){

            switch(index)
            {
                case 0: return System.AppDomain.CurrentDomain.BaseDirectory + "/Data/cell1.png";
                case 1: return System.AppDomain.CurrentDomain.BaseDirectory + "/Data/nutrient1.png";
                case 2: return System.AppDomain.CurrentDomain.BaseDirectory + "/Data/nutrient2.png";
               
                default: return System.AppDomain.CurrentDomain.BaseDirectory + "/Data/none.png";
            }

        }

        /// <summary>
        /// Gets a texture refernce, loading it into memory if it is not yet loaded
        /// </summary>
        /// <param name="index">Texture index</param>
        /// <param name="view">The context of the current OpenGL panel</param>
        /// <returns></returns>
        public static int GetTex(int index,int view)
        {
            UnsetTextures();
            loadTex(getTexName(index), index,view);

            return textures[view,index];
        }

        /// <summary>
        /// Unset all texture indices
        /// </summary>
        private static void UnsetTextures()
        {


            if (loaded == false)
            {
                for(int m =0;m<4;m++){
                    for(int n=0;n<100;n++)
                    {
                        textureSet[m,n]=false;
                    }
                }
                loaded = true;
            }

          

        }

        /// <summary>
        /// Release allocated texture memory
        /// </summary>
        public static void UnloadTextures(){

            if (loaded)
            {

                for(int view=0; view<4; view++)
                {

                    for (int n = 0; n < 100; n++)
                    {
                        if (textureSet[view,n])
                        {
                            GL.DeleteTextures(1, ref textures[view,n]);
                        }
                    }
                }
            }

        }



    }
}
