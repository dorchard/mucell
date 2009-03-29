using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    public enum InitialNutrientDistribution { UniformThroughout = 0, UniformSphere, DenselyCentredSphere};

    /// <summary>
    /// A class responsible for managing the Nutrient Field data structure
    /// </summary>
    public class NutrientField : ICloneable
    {
        //maximum concentrations of this nutrient in any unit volume in the environment
        private float maxPerUnit;
        private float minPerUnit;

        //whether or not the this nutrient field has yet been loaded
        private Boolean fieldLoaded;
        [XmlAttribute]
        public Boolean FieldLoaded
        {
            get { return fieldLoaded; }
            set { fieldLoaded = value; }
        }


        //the width, height and depth of the field array respectively
        private int[] dim;
        public int[] Dim
        {
            get { return dim; }
            set { dim = value; }
        }



        /* The concentration field. field[0,0,0] represents a cube of 
         * size (1 / resolution)^3 with a world cooridnate centre of (0,0,0)
         *  
         * 
         */
        private float[][][] field;
        public float[][][] Field
        {
            get { return field; }
            set { field = value; }
            
        }


        //the ID (index) of this group in the environment
        private int index;
        [XmlAttribute]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }


        //The name of this nutrient field
        private String name;
        [XmlAttribute]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }



        //The source position of the nutrient in the environment
        public Vector3 InitialPosition;


        /// <summary>
        /// How to colour the nutrient field (tinting)
        /// </summary>
        private System.Drawing.Color col;
        [XmlIgnore]
        public System.Drawing.Color Col
        {
            get { return col; }
            set { col = value; }
        }

        [XmlAttribute]
        public String Colour
        {
            get { return this.col.ToString(); }
            set { this.col = System.Drawing.Color.FromName(value); }
        }

        /// <summary>
        /// How do initally distribute the nutrients in the env
        /// </summary>
        private InitialNutrientDistribution initialDistribution;
        [XmlAttribute]
        public InitialNutrientDistribution InitialDistribution
        {
            get { return initialDistribution; }
            set { initialDistribution = value; }
        }

        /// <summary>
        /// Quantity of the nutrient to add to the environment initally
        /// </summary>
        private float initialQuantity;
        [XmlAttribute]
        public float InitialQuantity
        {
            get { return initialQuantity; }
            set { initialQuantity = value; }
        }

        /// <summary>
        /// Radius of the sphere where to initally distribute the nutrients
        /// </summary>
        private float initialRadius;
        [XmlAttribute]
        public float InitialRadius
        {
            get { return initialRadius; }
            set { initialRadius = value; }
        }

        /// <summary>
        /// Rate at which this nutrient diffuses within the env
        /// </summary>
        private float diffusionRate;
        [XmlAttribute]
        public float DiffusionRate
        {
            get { return diffusionRate; }
            set { diffusionRate = value; }
        }

        /// <summary>
        /// Data points per unit of the boundary
        /// </summary>
        private float resolution;
        [XmlAttribute]
        public float Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        //offset of the world coord system
        private Vector3 worldOffset;
        public Vector3 WorldOffset
        {
            get { return worldOffset; }
            set { worldOffset = value; }
        }


        /*
         * Note: cube dimesions are stored instead of derived directly because
         * they need to be accessed frequently during diffusion calculations
         */


        //volume of a single cube in the field array
        private float cubeVolume;
        [XmlAttribute]
        public float CubeVolume
        {
            get { return cubeVolume; }
            set { cubeVolume = value; }
        }

        //cross-sectional area of a single cube in the field array
        private float cubeArea;
        [XmlAttribute]
        public float CubeArea
        {
            get { return cubeArea; }
            set { cubeArea = value; }
        }

        //length of one of the sides of a single cube int he field array
        private float cubeLength;
        [XmlAttribute]
        public float CubeLength
        {
            get { return cubeLength; }
            set { cubeLength = value; }
        }




        /// <summary>
        /// Clones a NutrientField
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            NutrientField newInstance = new NutrientField(this.index);
            newInstance.Col = this.col;
            newInstance.CubeArea = this.cubeArea;
            newInstance.CubeLength = this.cubeLength;
            newInstance.CubeVolume = this.cubeVolume;
            newInstance.DiffusionRate = this.diffusionRate;
            newInstance.Dim = (int[])this.dim.Clone();
            newInstance.Field = (float[][][])this.field.Clone();
            newInstance.FieldLoaded = this.FieldLoaded;
            newInstance.InitialDistribution = this.initialDistribution;
            newInstance.InitialPosition = this.InitialPosition;
            newInstance.InitialQuantity = this.InitialQuantity;
            newInstance.InitialRadius = this.InitialRadius;
            newInstance.Name = this.name;
            newInstance.Resolution = this.resolution;
            newInstance.WorldOffset = this.worldOffset;

            return newInstance;
        }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public NutrientField(int index)
        {
            this.maxPerUnit = 1.0f;
            this.minPerUnit = 0.0f;
            this.index = index;
            this.name = "nutrient" + index;
            
            this.initialDistribution = InitialNutrientDistribution.UniformThroughout;
            this.initialRadius = 30f;
            this.diffusionRate = 1f;
            this.initialQuantity = 1.0f;
            this.resolution = 0.25f;
            this.dim = new int[3];
            this.dim[0] = this.dim[1] = this.dim[2] = 1;

            this.field = new float[1][][];
            this.field[0] = new float[1][];
            this.field[0][0] = new float[1];

            //this.field = new float[1, 1, 1];

            this.cubeArea = 1.0f;
            this.cubeLength = 1.0f;
            this.cubeVolume = 1.0f;

            this.col = System.Drawing.Color.CornflowerBlue;
        }

        public NutrientField()
        {
            this.maxPerUnit = 1.0f;
            this.minPerUnit = 0.0f;
            this.index = index;
            this.name = "nutrientloaded";

            this.initialDistribution = InitialNutrientDistribution.UniformThroughout;
            this.initialRadius = 30f;
            this.diffusionRate = 1f;
            this.initialQuantity = 1.0f;
            this.resolution = 0.25f;
            this.dim = new int[3];
            this.dim[0] = this.dim[1] = this.dim[2] = 1;

            this.field = new float[1][][];
            this.field[0] = new float[1][];
            this.field[0][0] = new float[1];

            //this.field = new float[1, 1, 1];

            this.cubeArea = 1.0f;
            this.cubeLength = 1.0f;
            this.cubeVolume = 1.0f;

            this.col = System.Drawing.Color.CornflowerBlue;
        }

        /// <summary>
        /// Converts from field coordinates to world coordinates
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public Vector3 FieldToWorldCoord(float i, float j, float k)
        {
            Vector3 v;

            

            v.x = i / resolution + worldOffset.x;
            v.y = j / resolution + worldOffset.y;
            v.z = k / resolution + worldOffset.z;

            return v;

        }



        /// <summary>
        /// Returns the approxiamte nutrient level at a specific location, suitable
        /// for graphical output, but not for simulation purposes
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public float GetNutrientLevelApprox(float x, float y, float z)
        {
            //which cube the given point falls into
            int i = (int)((x - worldOffset.x) * resolution);
            int j = (int)((y - worldOffset.y) * resolution);
            int k = (int)((z - worldOffset.z) * resolution);


            if (i < 0 || i >= this.dim[0] ||
                j < 0 || j >= this.dim[1] ||
                k < 0 || k >= this.dim[2])
            {
                return 0;
            }

            return field[i][j][k] / this.cubeVolume;

        }

        /// <summary>
        /// Returns the nutrient level at a specific location
        /// </summary>
        /// <returns></returns>
        public float GetNutrientLevel(Vector3 pos)
        {
            return GetNutrientLevel(pos.x, pos.y, pos.z);

        }


        /// <summary>
        /// Returns the nutrient level at a particular point in 
        /// moles per litre.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public float GetNutrientLevel(float x,float y, float z)
        {

			//which cube the given point falls into
            int midx = (int)((x - worldOffset.x )*resolution);
            int midy = (int)((y - worldOffset.y) * resolution);
            int midz = (int)((z - worldOffset.z) * resolution);
            
            System.Console.WriteLine("x, y, z="+x+","+y+","+z);
            System.Console.WriteLine("midx, midy, midz="+midx+","+midy+","+midz);

            /*
             * Concentration is computed using a gaussian-style function:
             * A weighted average of all concentrations of the 3x3x3 points surrounding
             * the given point is computed. 
             * 
             */

            //Need to first compute the total sum of coefficients so that weighting can be scaled to sum to 1
            float sumOfExp = 0;
            float val = 0;
            Vector3 pos;

          
            for (int m = 0; m < 2; m++)
            {


                for (int i = midx - 2; i <= midx + 2; i++)
                {
                    for (int j = midy - 2; j <= midy + 2; j++)
                    {
                        for (int k = midz - 2; k <= midz + 2; k++)
                        {

        
                            if (i < 0 || i >= this.dim[0] ||
                                j < 0 || j >= this.dim[1] ||
                                k < 0 || k >= this.dim[2])
                            {
                                //out of bounds

                            }
                            else
                            {

                                pos = FieldToWorldCoord((float)i, (float)j, (float)k);
                                pos.x -= x;
                                pos.y -= y;
                                pos.z -= z;
							
                                float dist = pos.magnitude();
                                
                                System.Console.WriteLine("i,j,k = "+i+","+j+","+k+" positions (x,y,z)="+pos.x+","+pos.y+","+pos.z+" dist = "+dist);
                                
                                //float exp = (float)Math.Pow(2, -5 * dist *resolution);
                                float exp = (float)Math.Pow(Math.E, -1 * dist / this.CubeLength);
                
                                if (m == 0)
                                {
                                    //first pass, sum exp values in order to generating a weighting kernel that sums to 1
                                    sumOfExp += exp;
                                }
                                else
                                {   
                                    //where exp/sumOfExp is the weighting coefficient
                                    
                                    val +=  (exp/sumOfExp) *  (field[i][j][k]); /// this.cubeVolume );
                                    System.Console.WriteLine("field["+i+"]["+j+"]["+k+"] = "+field[i][j][k]);
                                }

                            }

                        } //end of loop k
                    } //end of loop j
                } //end of loop i


            } //end of loop m

			System.Console.WriteLine("sum of exp was = "+sumOfExp);
         	System.Console.WriteLine("val returned from nutrient thing = "+val);
            return val;


        }


        /// <summary>
        /// Computes the dimensions of the field array, loads it into memory, 
        /// and sets initial values;
        /// </summary>
        public void InitField(Boundary bounds)
        {
            TestRigs.ErrorLog.LogError("bounds x: " + bounds.Width + " y: " + bounds.Height + " z: " + bounds.Depth);

            UpdateDimensions(bounds);

            this.field = new float[dim[0]][][];
            for (int i = 0; i < this.field.Length; i++)
            {
                this.field[i] = new float[dim[1]][];
                for (int j = 0; j < this.field[i].Length; j++)
                {
                    this.field[i][j] = new float[dim[2]];
                }
            }
            //this.field = new float[dim[0], dim[1], dim[2]];


            //init all values to 0
            for (int i = 0; i < dim[0]; i++)
            {
                for (int j = 0; j < dim[1]; j++)
                {
                    for (int k = 0; k < dim[2]; k++)
                    {
                        if (bounds.InsideBoundary(FieldToWorldCoord((float)i, (float)j, (float)k)))
                        {
                            field[i][j][k] = 0.0f;
                        }
                    }
                }
            }


            switch (initialDistribution)
            {
                case InitialNutrientDistribution.UniformThroughout:
                    {

                        /*
                         * Uniformly distribute the nutrient in the environment
                         * by counting the number of cubes inside the env, and then
                         * adding an equal fraction of nutrients to each of these 
                         * field cubes. 
                         */

                        //int cubesInside = 0;

                        //for (int m = 0; m < 2; m++)
                        //{


                            //m=0 : begin by counting how many field cubes are inside the boundary
                            //m=1 : Distribute evenly amongst all cubes inside the boundary
                            for (int i = 0; i < dim[0]; i++)
                            {
                                for (int j = 0; j < dim[1]; j++)
                                {
                                    for (int k = 0; k < dim[2]; k++)
                                    {
                                        if (bounds.InsideBoundary(FieldToWorldCoord((float)i, (float)j, (float)k)))
                                        {
                                            //if (m == 0)
                                            //{
                                            //    cubesInside++;
                                            //}
                                            //else
                                            //{
                                            	field[i][j][k] =  InitialQuantity;// / cubesInside;
                                            //}
                                        }
                                    }
                                }
                            }
                        //}

                
                        break;
                    }
                case InitialNutrientDistribution.DenselyCentredSphere:
                    {

                        Boundary innerSphere = new Boundary();
                        innerSphere.Radius = this.InitialRadius;
                        innerSphere.Shape = BoundaryShapes.Sphere;

                        /* 
                         * Here we use a similar techique, however the quantity
                         * to be added decreases exponentially with the distance 
                         * from the centre of the sphere. We need to first calculate
                         * the sum of these exponentials so that we know the scaling
                         * factor for the final values. By doing this we ensure that
                         * exactly the correct quantity of nutrients is added.
                         */
                        //float sumOfExp = 0;

                        //for (int m = 0; m < 2; m++)
                        //{



                            for (int i = 0; i < dim[0]; i++)
                            {
                                for (int j = 0; j < dim[1]; j++)
                                {
                                    for (int k = 0; k < dim[2]; k++)
                                    {
                                        //test that location is inside the env boundary
                                        if (bounds.InsideBoundary(FieldToWorldCoord((float)i, (float)j, (float)k)))
                                        {
                                            Vector3 pos = FieldToWorldCoord((float)i, (float)j, (float)k);
                                            pos.x -= this.InitialPosition.x;
                                            pos.y -= this.InitialPosition.y;
                                            pos.z -= this.InitialPosition.z;

                                            if (innerSphere.InsideBoundary(pos))
                                            {
                                                float dist = pos.magnitude();
                                                //if (m == 0)
                                                //{

                                                    //begin by summing total distances from each point to the centre of the sphere
                                                //    sumOfExp += (float)Math.Pow(2, -5 * dist / this.InitialRadius);
                                                    //sumOfExp += 1;
                                                //}
                                                //else
                                                //{
                                                    //once counted, distribute evenly
                                                    //System.Console.WriteLine("(i,j,k)=("+i+","+j+","+k+") dist = "+dist+" q="+InitialQuantity+" r="+this.InitialRadius +
                                                    //	" 2 ^ ? = 2 ^ "+(-4 * dist / this.InitialRadius)+ " sumofexp="+sumOfExp+" 2^?/s="+
                                                    //	((float)Math.Pow(2, -4 * dist / this.InitialRadius) / sumOfExp)+" final = "+(InitialQuantity * (float)Math.Pow(2, -4 * dist / this.InitialRadius) / sumOfExp));

                                                    //field[i][j][k] = InitialQuantity * (float)Math.Pow(2, -5 * dist / this.InitialRadius) / sumOfExp;
                                                    field[i][j][k] = InitialQuantity * Math.Abs((this.initialRadius - dist)/(this.initialRadius)); 
                                                    //field[i][j][k] = InitialQuantity / sumOfExp;
                                                //}
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    //}


                    break;



                /*
                 * Nutrient quantity is evenly distrubted in a sphere, again
                 * we use multiple passes to ensure that exactly the correct
                 * quantity is added
                 */
                case InitialNutrientDistribution.UniformSphere:
                    {

                        Boundary innerSphere = new Boundary();
                        innerSphere.Radius = this.InitialRadius;
                        innerSphere.Shape = BoundaryShapes.Sphere;


                        //int cubesInside = 0;

                        //for (int m = 0; m < 2; m++)
                        //{


                            
                            for (int i = 0; i < dim[0]; i++)
                            {
                                for (int j = 0; j < dim[1]; j++)
                                {
                                    for (int k = 0; k < dim[2]; k++)
                                    {
                                        //test that location is inside the env boundary
                                        if (bounds.InsideBoundary(FieldToWorldCoord((float)i, (float)j, (float)k)))
                                        {
                                            Vector3 pos = FieldToWorldCoord((float)i, (float)j, (float)k);
                                            pos.x -= this.InitialPosition.x;
                                            pos.y -= this.InitialPosition.y;
                                            pos.z -= this.InitialPosition.z;

                                            //test that location is inside desired sphere
                                            if (innerSphere.InsideBoundary(pos))
                                            {
                                                //if (m == 0)
                                                //{   
                                                //    //begin by counting how many field cubes are inside the boundary and sphere
                                                //    cubesInside++;
                                                //}
                                                //else
                                                //{
                                                    //once counted, distribute evenly
                                                    field[i][j][k] =  InitialQuantity; /// cubesInside;
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                        //}

                        break;
                    }
            }


            this.fieldLoaded = true;

        }


        /*
         * Estimates the value by which to scale the alpha value
         * of the polygons when drawing this nutrient field 
         */ 
        public float EstimateIntensityScale()
        {
            if (!this.FieldLoaded)
            {
                return 0;
            }

            float totalVal = 0;
            int cnt = 0;

            for (int i = 0; i < dim[0]; i++)
            {
                for (int j = 0; j < dim[1]; j++)
                {
                    for (int k = 0; k < dim[2]; k++)
                    {

                        if (field[i][j][k] != 0)
                        {
                            totalVal += field[i][j][k] / this.cubeVolume;
                            cnt++;
                        }


                    }
                }
            }


            return 0.4f / ((totalVal / (float)cnt));


        }


        /// <summary>
        /// Diffuses nutrients from cube (x1,y1,z1) to (x2,y2,z2)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="z1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z2"></param>
        private void diffuse(int x1, int y1, int z1, int x2, int y2, int z2, float timeStep)
        {
            //check that both positions are in the bounds, and adj
            if (x1 < 0 || y1 < 0 || z1 < 0 || x2 < 0 || y2 < 0 || z2 < 0 ||
                x1 >= dim[0] || y1 >= dim[1] || z1 >= dim[2] ||
                x2 >= dim[0] || y2 >= dim[1] || z2 >= dim[2] ||
                (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)+(z1 - z2) * (z1 - z2) != 1   //points are not exactly a distance 1 apart
                )
            {
                return;
            }


            //Calculate gradient, and diffuse nutrients accordingly
            float valA = field[x1][y1][z1];
            float valB = field[x2][y2][z2];

            float delta = timeStep*50*(valB - valA) * cubeArea * diffusionRate;

            field[x1][y1][z1] += delta;
            field[x2][y2][z2] -= delta;


        }




        /// <summary>
        /// For each cube, diffuse to 9 neighbors. Boundary conditions 
        /// are checked within the diffusion method.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeStep"></param>
        public void DoTimeStep(double time, double timeStep)
        {
            if (!this.FieldLoaded || diffusionRate == 0.0f)
            {
                return;
            }
            float tim = (float)timeStep;


            //for every field cube, compute diffision to the 8 adj cubes

            for (int i = 0; i < dim[0]; i++)
            {
                for (int j = 0; j < dim[1]; j++)
                {
                    for (int k = 0; k < dim[2]; k++)
                    {




                        for (int p = -1; p <= 1; p++)
                        {
                            for (int q = -1; q <= 1; q++)
                            {
                                for (int r = -1; r <= 1; r++)
                                {
                                    diffuse(i, j, k,
                                            i + p, j + q, k + r, tim);

                                }
                            }
                        }



                    }
                }
            }




        }





        /// <summary>
        /// Given the boundary, computes the dimensions of the field array
        /// at its current resolution
        /// </summary>
        /// <param name="bounds"></param>
        
        // There are currently lots of assumptions and restrictions
        // ONLY CUBOID ENVIRONMENTS ARE ALLOWED!
        public void UpdateDimensions(Boundary bounds)
       {
            
            switch (bounds.Shape)
            {
                case BoundaryShapes.Cuboid:
                	this.dim[0] = (int)Math.Ceiling(bounds.Width * this.resolution);
                    this.dim[1] = (int)Math.Ceiling(bounds.Height * this.resolution);
                    this.dim[2] = (int)Math.Ceiling(bounds.Depth * this.resolution);
                    this.worldOffset.x = -bounds.Width / 2;
                    this.worldOffset.y = -bounds.Height / 2;
                    this.worldOffset.z = -bounds.Depth / 2;
                    break;

                case BoundaryShapes.Cylinder:
                    this.dim[0] =  this.dim[2] = 1 + (int)Math.Ceiling(2*bounds.Radius * this.resolution);
                    this.dim[1] = 1 + (int)Math.Ceiling(bounds.Height * this.resolution);
                    this.worldOffset.x = this.worldOffset.z = -bounds.Radius;
                    this.worldOffset.y = -bounds.Height / 2;
                    break;

                case BoundaryShapes.Sphere:
                	// previously: 
                	// this.dim[0] = this.dim[1] = this.dim[2] = 1 + (int)Math.Ceiling(2*bounds.Radius * this.resolution);
                	
                	// changed 26/04/2009
                	
                    this.dim[0] = this.dim[1] = this.dim[2] = (int)Math.Ceiling(2*bounds.Radius * this.resolution);
                    this.worldOffset.x = this.worldOffset.y = this.worldOffset.z = -bounds.Radius;
                    
                    break;
            }

			// these calculations imply only a cuboid is allowed
			// Normalise so that environment is 1x1x1
			this.cubeLength = (float)(1.0f / dim[0]);
			this.cubeArea = (float)(Math.Pow(this.cubeLength, 2));
			this.cubeVolume = (float)(Math.Pow(this.cubeLength, 3));
		
			// old code that was fixed for 100*100*100 cuboid
            //this.cubeVolume = (float)Math.Pow( 1.0f / (resolution*100) , 3.0f );
            //this.cubeArea = (float)Math.Pow(1.0f / (resolution*100), 2.0f);
            //this.cubeLength = (float)1.0f / (resolution*100);

            this.FieldLoaded = false;
        }



        /// <summary>
        /// Estimates space consumption of this field in MB
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public float EstimateSpaceConsumption(Boundary bounds)
        {
            return (float)(dim[0] * dim[1] * dim[2] * 4) / 1048576;
        }


        /// <summary>
        /// Returns the concentration of this nutrient field at the given point in space
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float getNutrientsAtPoint(Vector3 point)
        {
            //return distance from the centre for the moment
            return point.magnitude();
        }

        /// <summary>
        /// Returns the name of this Nutrient Field
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }

    }
}
