using UnityEngine;

/// <summary>Class <c>DiamondSquareGen</c> An outer container for the diamond square algorithm </summary>
[RequireComponent(typeof(Terrain))][RequireComponent(typeof(TerrainCollider))] //terrain component stores height-map data
public class DiamondSquareGen : MonoBehaviour
{
    DiamondSquareLogic diamondSquareLogic;
    //[SerializeField] float heightDepthRandomness = 0.5f; //the starting upper range of the height-map

    public float heightDepthRandomness = 0.5f;

    public float MagnitudeScale = 0.6f;

    void Start()
    {
        diamondSquareLogic = new DiamondSquareLogic(GetComponent<Terrain>().terrainData, heightDepthRandomness, MagnitudeScale);

        diamondSquareLogic.PerformGeneration();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) //arbitrary key
        {
            diamondSquareLogic.PerformGeneration();
        }
    }

    public void UpdateTerrain()
    {
        diamondSquareLogic = new DiamondSquareLogic(GetComponent<Terrain>().terrainData, heightDepthRandomness, MagnitudeScale);

        diamondSquareLogic.PerformGeneration();
    }

     class DiamondSquareLogic
    {
        #region Init
        readonly TerrainData meshData; //access to vertices's

        readonly int WxH; //by default all terrain matches conveniently matches DS' requirement of w x h 2^n + 1
        readonly float startHeightDepth; //This value is the i

        readonly float heightNormalisation = 10;

        float[,] heights; //2D arr used to assign heights

        public float heightDepthRNG; //max ~5 based of internal terrain sky-box

        readonly float Magnitude; //Scale factor


        #endregion

        public DiamondSquareLogic(TerrainData meshData, float heightDepthRNG, float MagnitudeScaler)  
        {
            this.meshData = meshData;
            WxH = meshData.heightmapResolution; //one could use the height map resolution value directly
            this.heightDepthRNG = heightDepthRNG;
            startHeightDepth = heightDepthRNG;
            Magnitude = MagnitudeScaler;
        }

        /// <summary>Method <c>PerformGeneration</c>The steps needed for the algorithm </summary>
        public void PerformGeneration()
        {
            InitCornerRandomness(); //STEP ONE (Corner step)
            PerformDiamondAndSquareStep(); //STEP TWO (Diamond + Square)
        }

        /// <summary>Method <c>InitCornerRandomness</c> Sets the four corners of the terrain to constrained random ranges </summary>
        void InitCornerRandomness()
        {
            heights = new float[WxH, WxH]; //this array can become exponentially large relative to resolution (default resolution is 513)
            heightDepthRNG = startHeightDepth;

            //Four corners are set
            heights[0, 0] = Random.Range(-heightDepthRNG/heightNormalisation,heightDepthRNG/heightNormalisation); 
            heights[WxH - 1, 0] = Random.Range(-heightDepthRNG/heightNormalisation,heightDepthRNG/heightNormalisation); 
            heights[0, WxH - 1] = Random.Range(-heightDepthRNG/heightNormalisation,heightDepthRNG/heightNormalisation);
            heights[WxH - 1, WxH - 1] = Random.Range(-heightDepthRNG/heightNormalisation,heightDepthRNG/heightNormalisation);
            
        }

        //Diamond and square steps growth is exponential (because the grid is being "cut" in chunks), runtime is exponential (O(2^N)) where N = the height map resolution
        void PerformDiamondAndSquareStep() 
        {
            int stepSize = WxH - 1, x, y;
            
            while (stepSize > 1)
            {
                heightDepthRNG *= Magnitude; //reduce magnitude of randomness by scaling factor;
                //Debug.Log(Magnitude);
                int halfStep = stepSize / 2; //division by two is safe as the height-map res (WxH - 1) is to a power of 2

                float localAverage;

                //DIAMOND STEP 
                for (x = 0; x < WxH - 1; x+=stepSize) //in total this loop will exponentially increase
                {
                    for (y = 0; y < WxH - 1;y+=stepSize)
                    {
                        localAverage = (heights[x, y] + heights[x + stepSize, y] + heights[x, y + stepSize] + heights[x + stepSize, y + stepSize]) / 4; //average is totaled

                        heights[x + halfStep, y + halfStep] = localAverage + Random.Range(-heightDepthRNG, heightDepthRNG);
                    }
                }

                //SQUARE STEP (Get points in-between the diamond corner values)
                for (x = 0; x < WxH; x += halfStep) //we get to the mini clusters of squares by getting a decreased half step each time
                {
                    for (y = (x + halfStep) % stepSize; y < WxH - 1; y += stepSize)
                    {                       
                        //modulo operations for each dir to ensure no use of try-catch
                        localAverage = heights[(x - halfStep + WxH - 1) % (WxH - 1), y];
                        localAverage += heights[(x + halfStep) % (WxH - 1), y];
                        localAverage += heights[x, (y + halfStep) % (WxH - 1)];
                        localAverage += heights[x, (y - halfStep + WxH - 1) % (WxH - 1)];

                        localAverage /= 4.0f; //average is calculated 
                        
                        heights[x, y] = localAverage + Random.Range(-heightDepthRNG, heightDepthRNG);

                    }
                }

                stepSize /= 2; //search space is reduced

            }
            //Since the rendering is not iterative the performance is not hit much at all
            meshData.SetHeights(0, 0, heights); //once all height-map data is calculated, render using arr 
        }

    }
}
