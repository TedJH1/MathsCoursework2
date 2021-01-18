using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GUIInteraction : MonoBehaviour
{
    // **ONCLICK/SLIDE FUNCTIONALITY**

    public DiamondSquareGen GenParementers;
    public MidpointDisplacement MidParemeters;

    public Material[] materials;
    public Terrain terrain;

    public TextMeshProUGUI RockHeight;
    public TextMeshProUGUI MagnitudeLevel;
    public TextMeshProUGUI WaterLevel;
    public TextMeshProUGUI SpreadLevel;
    public TextMeshProUGUI SpreadReductionLevel;

    private double RockHeightChange = 0.5;
    private double MagnitudeLevelChange = 0.6;
    private int SeaChange = -300;
    private double SpreadChange = 0.3;
    private double SpreadReductionChange = 0.5;

    public Transform Water;
    Vector3 IncreaseLevel = new Vector3(0f, 10f, 0f);

    public CanvasGroup ui;

    // Start is called before the first frame update
    void Start() //Set initial values for UI
    {
        RockHeight.text = "Rock height: " + RockHeightChange;
        MagnitudeLevel.text = "Magnitude: " + MagnitudeLevelChange;
        WaterLevel.text = "Sea level: " + SeaChange;
        SpreadLevel.text = "Spread level: " + SpreadChange;
        SpreadReductionLevel.text = "Spread Reduction: " + SpreadReductionChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) //arbitrary key
        {
            //For testing
        }
    }

    public void Refresh()
    {
        GenParementers.UpdateTerrain();
    }

    public void RefreshMidpoint()
    {
        MidParemeters.UpdateTerrain();
    }

    public void ReturnToMainMenu() => SceneManager.LoadScene(0);

    #region Diamond Parameters

    #region Rock Height
    public void IncreaseHeightDepthRandomness() //Increases height range by 0.1 to a max value of 1
    {
        if (GenParementers.heightDepthRandomness < 1f)
        {
            GenParementers.heightDepthRandomness += 0.1f;
            GenParementers.UpdateTerrain();
            RockHeight.text = "Rock height: " + (RockHeightChange += 0.1);
        }
        else
        {
            GenParementers.heightDepthRandomness = 1f;
        }

        //RockHeight.text = "Rock heigh: " + GenParementers.heightDepthRandomness.ToString();
        //Debug.Log(GenParementers.heightDepthRandomness);
    }

    public void DecreaseHeightDepthRandomness() //Decrements height range by 0.1 to a min value of 0.1
    {
        if (GenParementers.heightDepthRandomness > 0.19f)
        {
            GenParementers.heightDepthRandomness -= 0.1f;
            GenParementers.UpdateTerrain();
            RockHeight.text = "Rock height: " + (RockHeightChange -= 0.1);
        }
        else
        {
            GenParementers.heightDepthRandomness = 0.1f;
        }

        //RockHeight.text = "Rock height: " + GenParementers.heightDepthRandomness.ToString();
        //Debug.Log(GenParementers.heightDepthRandomness);
    }
    #endregion

    #region Magnitude
    public void IncreaseMagnitude()
    {
        if(GenParementers.MagnitudeScale < 0.9f)
        {
            GenParementers.MagnitudeScale += 0.1f;
            GenParementers.UpdateTerrain();
            MagnitudeLevel.text = "Magnitude: " + (MagnitudeLevelChange += 0.1);
        } else
        {
            GenParementers.MagnitudeScale = 0.9f;
        }

        //Debug.Log(GenParementers.MagnitudeScale);
    }

    public void DecreaseMagnitude()
    {
        if (GenParementers.MagnitudeScale > 0.19f)
        {
            GenParementers.MagnitudeScale -= 0.1f;
            GenParementers.UpdateTerrain();
            MagnitudeLevel.text = "Magnitude: " + (MagnitudeLevelChange -= 0.1);
        }
        else
        {
            GenParementers.MagnitudeScale = 0.1f;
        }

        //Debug.Log(GenParementers.MagnitudeScale);
    }
    #endregion

    #region Water Level

    public void IncreaseWaterLevel()
    {
        if(Water.transform.position.y < -200)
        {
            Water.transform.position += IncreaseLevel;
            WaterLevel.text = "Sea level: " + (SeaChange += 10);
        }
        else
        {
            Water.transform.position = Water.transform.position;
        }

        //Debug.Log(SeaChange);
    }

    public void DecreaseWaterLevel()
    {
        if (Water.transform.position.y > -300)
        {
            Water.transform.position -= IncreaseLevel;
            WaterLevel.text = "Sea level: " + (SeaChange -= 10);
        }
        else
        {
            Water.transform.position = Water.transform.position;
        }

       // Debug.Log(SeaChange);
    }

    #endregion

    #endregion

    #region Diamond Presets

    public void SetBankPreset(int options)
    {
        //Bank Preset
        if(options == 1)
        {
            GenParementers.heightDepthRandomness = 0.4f;
            GenParementers.MagnitudeScale = 0.2f;
            Water.transform.position = new Vector3(0, -290, 0);

            //Set UI Variables
            RockHeight.text = "Rock height: " + 0.4;
            MagnitudeLevel.text = "Magnitude: " + 0.2;
            WaterLevel.text = "Sea level: " + -290;

            //Update UI variables
            RockHeightChange = 0.4;
            MagnitudeLevelChange = 0.2;
            SeaChange = -290;

            GenParementers.UpdateTerrain();
        }

        //Mountain Preset
        if (options == 2)
        {
            GenParementers.heightDepthRandomness = 1f;
            GenParementers.MagnitudeScale = 0.6f;
            Water.transform.position = new Vector3(0, -280, 0);

            //Set UI Variables
            RockHeight.text = "Rock height: " + 1;
            MagnitudeLevel.text = "Magnitude: " + 0.6;
            WaterLevel.text = "Sea level: " + -280;

            //Update UI variables
            RockHeightChange = 1;
            MagnitudeLevelChange = 0.6;
            SeaChange = -280;

            GenParementers.UpdateTerrain();
        }
    }

    #endregion

    #region Midpoint Paremeters

    #region Spread
    public void IncreaseSpread() //Increases spread by 0.1 to a max value of 0.5
    {
        if(MidParemeters.spread < 0.5f)
        {
            MidParemeters.spread += 0.1f;
            MidParemeters.UpdateTerrain();
            SpreadLevel.text = "Spread: " + (SpreadChange += 0.1);
        } else
        {
            MidParemeters.spread = 0.5f;
        }
   
    }

    public void DecreaseSpread() //Increases spread by 0.1 to a max value of 0.5
    {
        if (MidParemeters.spread > 0.2f)
        {
            MidParemeters.spread -= 0.1f;
            MidParemeters.UpdateTerrain();
            SpreadLevel.text = "Spread: " + (SpreadChange -= 0.1);
        }
        else
        {
            MidParemeters.spread = 0.1f;
        }

        //Debug.Log(MidParemeters.spread);

    }
    #endregion

    #region Spread Reduction
    public void IncreaseSpreadReduction() //Increases spread by 0.1 to a max value of 0.5
    {
        if (MidParemeters.spreadReductionRate < 0.5f)
        {
            MidParemeters.spreadReductionRate += 0.1f;
            MidParemeters.UpdateTerrain();
            SpreadReductionLevel.text = "Spread Reduction: " + (SpreadReductionChange += 0.1);
        }
        else
        {
            MidParemeters.spreadReductionRate = 0.5f;
        }

    }

    public void DecreaseSpreadReduction() //Increases spread by 0.1 to a max value of 0.5
    {
        if (MidParemeters.spreadReductionRate > 0.2f)
        {
            MidParemeters.spreadReductionRate -= 0.1f;
            MidParemeters.UpdateTerrain();
            SpreadReductionLevel.text = "Spread Reduction: " + (SpreadReductionChange -= 0.1);
        }
        else
        {
            MidParemeters.spreadReductionRate = 0.1f;
        }

        Debug.Log(MidParemeters.spreadReductionRate);

    }
    #endregion

    #endregion

    #region Midpoint Presets

    public void MidpointPresets(int options)
    {
        if (options == 1)
        {
            MidParemeters.spread = 0.1f;
            MidParemeters.spreadReductionRate = 0.2f;
            Water.transform.position = new Vector3(0, -300, 0);

            //Set UI Variables
            SpreadLevel.text = "Spread level: " + 0.1;
            SpreadReductionLevel.text = "Spread Reduction: " + 0.2;
            WaterLevel.text = "Sea level: " + -300;

            //Update UI Variables
            SpreadChange = 0.1;
            SpreadReductionChange = 0.2;
            SeaChange = -300;

            MidParemeters.UpdateTerrain();

        }

        if (options == 2)
        {
            MidParemeters.spread = 0.4f;
            MidParemeters.spreadReductionRate = 0.5f;
            Water.transform.position = new Vector3(0, -260, 0);

            //Set UI Variables
            SpreadLevel.text = "Spread level: " + 0.4;
            SpreadReductionLevel.text = "Spread Reduction: " + 0.5;
            WaterLevel.text = "Sea level: " + -260;

            //Update UI Variables
            SpreadChange = 0.4;
            SpreadReductionChange = 0.5;
            SeaChange = -260;

            MidParemeters.UpdateTerrain();
        }
    }

    #endregion

    #region Materials

    public void SetMaterial(int id)
    {
        // Sets material colour of the terrain
        terrain.materialTemplate = materials[id];
    }

    #endregion

    #region Toggle UI

    public void ToggleUI()
    {
        // Toggles visibility of main UI
        ui.alpha = ui.alpha == 1 ? 0 : 1;
    }

    #endregion

}
