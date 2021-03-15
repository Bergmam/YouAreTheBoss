using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public Image backgroundImage;
    // public Material backgroundMaterial;

    // private Material backgroundMaterial;
    public void Start()
    {
        // backgroundMaterial = backgroundImage.material;
        this.CreateRandomBackground();
    }

    public void CreateRandomBackground()
    {
        Material tempMaterial = Instantiate(backgroundImage.material);
        tempMaterial.SetColor("_Color", Random.ColorHSV());
        tempMaterial.SetColor("_BackgroundColor", Random.ColorHSV());
        tempMaterial.SetFloat("_Scale", Random.Range(2f, 5.0f));
        tempMaterial.SetFloat("_TwirlStrength", Random.Range(10f, 40.0f));
        tempMaterial.SetFloat("_Speed", Random.Range(0f, 10.0f));

        Vector2 bossScreenPos = Camera.main.WorldToScreenPoint(GameObject.Find("Boss").transform.position);
        Vector2 bossAnchoredPos = new Vector2(bossScreenPos.x / Screen.width, bossScreenPos.y / Screen.height);
        tempMaterial.SetVector("_AnchoredPosition", bossAnchoredPos);

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
