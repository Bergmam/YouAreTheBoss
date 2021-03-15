using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public Image backgroundImage;
    // public Material backgroundMaterial;

    // private Material backgroundMaterial;
    public void Start() {
        // backgroundMaterial = backgroundImage.material;
        this.CreateRandomBackground();
    }

    public void CreateRandomBackground() {

        Material tempMaterial = Instantiate(backgroundImage.material);
        // Color
        tempMaterial.SetColor("Color_E217BDB6", Random.ColorHSV());
        // BackgroundColor
        tempMaterial.SetColor("Color_55139648", Random.ColorHSV());
        // Scale
        tempMaterial.SetFloat("Vector1_5B33613A", Random.Range(2f, 5.0f));
        // TwirlStrength
        tempMaterial.SetFloat("Vector1_7A46A631", Random.Range(10f, 40.0f));
        // Speed
        tempMaterial.SetFloat("Vector1_6ED7C9C8", Random.Range(0f, 10.0f));
        backgroundImage.material = tempMaterial;
    }

    // public void SetBackgroundOne(){

    // }

    // public void SetBackgroundTwo(){

    // }

    // public void SetBackgroundThree(){

    // }

    // public void SetBackgroundFour(){

    // }
}
