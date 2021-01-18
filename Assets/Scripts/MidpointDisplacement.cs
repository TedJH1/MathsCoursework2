using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointDisplacement : MonoBehaviour
{
    // Start is called before the first frame update
    MidpointDisplacementLogic midpointDisplacement;
    //[SerializeField] float spread = 0.4f;
    //[SerializeField] float spreadReductionRate = 0.4f;

    public float spread = 0.4f;
    public float spreadReductionRate = 0.4f;

    void Start()
    {
        midpointDisplacement = new MidpointDisplacementLogic(GetComponent<Terrain>().terrainData, spread, spreadReductionRate);
        midpointDisplacement.PerformGen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            midpointDisplacement.PerformGen();
        }

    }

    public void UpdateTerrain()
    {
        midpointDisplacement = new MidpointDisplacementLogic(GetComponent<Terrain>().terrainData, spread, spreadReductionRate);
        midpointDisplacement.PerformGen();
    }

    /// <summary>Inner class <c>MidpointDisplacementLogic</c> The logic behind the algorithm </summary>
    public class MidpointDisplacementLogic
    {
        #region Init
        float[,] heights; //height data
        float spread; //amount of offset added
        float spreadReductionRate; //amount of offset decrease each time

        readonly int WxH; //height-map resolution
        readonly TerrainData td; //data holding terrain data, along with its respective vertices's
        readonly float initalSpread; //used in the reset of the algorithm
        #endregion

        public MidpointDisplacementLogic(TerrainData meshData, float spread, float spreadReductionRate)
        {
            WxH = meshData.heightmapResolution;
            td = meshData;
            this.spread = spread;
            initalSpread = spread;
            this.spreadReductionRate = spreadReductionRate;
        }

        /// <summary>Method <c>PerformGen</c> The steps needed to complete the algorithm </summary>
        public void PerformGen()
        {
            //All algorithmic steps
            InitRandomCorners();
            PerformSteps();
        }

        /// <summary>Method <c>InitRandomCorners</c> Set the four corners to random constraints </summary>
        void InitRandomCorners()
        {
            heights = new float[WxH, WxH];
            spread = initalSpread;

            //Four corners are set
            heights[0, 0] = Random.Range(-0.1f, 0.1f);
            heights[WxH - 1, 0] = Random.Range(-0.1f, 0.1f);
            heights[0, WxH - 1] = Random.Range(-0.1f, 0.1f);
            heights[WxH - 1, WxH - 1] = Random.Range(-0.1f, 0.1f);

            td.SetHeights(0, 0, heights); //height-map data is set 
        }

        /// <summary>Method <c>PerformSteps</c> The steps needed in the algorithm </summary>
        public void PerformSteps()
        {
            int stepSize = WxH - 1; //used in reducing step size

            while (stepSize > 1)
            {
                int halfStep = stepSize / 2; //division by two is safe as the height-map res (WxH - 1) is to a power of 2

                for (int x = 0; x < WxH - 1; x += stepSize) //in total this loop will exponentially increase
                {
                    for (int y = 0; y < WxH - 1; y += stepSize)
                    {

                        //Top midpoint
                        heights[x + halfStep, y] = ((heights[x, y] + heights[x + stepSize, y]) / 2) + Random.Range(-1.0f, 1f) * spread;

                        //Far right point
                        heights[x + stepSize, y + halfStep] = ((heights[x + stepSize, y] + heights[x + stepSize, y + stepSize]) / 2) + Random.Range(-1.0f, 1f) * spread;

                        //Far left point
                        heights[x, y + halfStep] = ((heights[x, y] + heights[x, y + stepSize]) / 2) + Random.Range(-1.0f, 1f) * spread;

                        //Bottom point
                        heights[x + halfStep, y + stepSize] = ((heights[x + stepSize, y + stepSize] + heights[x,y+stepSize]) / 2) + Random.Range(-1.0f, 1f) * spread;

                        heights[x + halfStep, y + halfStep] = ((heights[x + halfStep, y] + heights[x + stepSize, y + halfStep] + heights[x, y + halfStep] +
                            heights[x + halfStep, y + stepSize]) / 4) + Random.Range(-1.0f, 1f) * spread;
                    }
                }

                spread *= spreadReductionRate; //spread rate is reduced
                stepSize /= 2; //reduce quadrilateral 

            }

            td.SetHeights(0, 0, heights); //once performed update heights

        }


    }
}
