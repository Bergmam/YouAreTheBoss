using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBehaviour : MonoBehaviour
{

    private Image image;
    private float originalCooldownValue;
    private float cooldown;
    private Toggle parentButton;
    public bool Ready;

    void Awake()
    {
        this.Ready = true;
        this.parentButton = transform.parent.GetComponent<Toggle>();
        this.image = gameObject.GetComponent<Image>();
        if (this.image == null)
        {
            Destroy(this);
        }
        this.image.color = Parameters.AttACK_BUTTON_COOLDOWN_COLOR;

        //Make sure the image the script is attached to is actually filled radially.
        this.image.type = Image.Type.Filled;
        this.image.fillMethod = Image.FillMethod.Radial360;
    }

    void Update()
    {
        if (this.cooldown > 0)
        {
            this.cooldown -= Time.deltaTime;
            this.image.fillAmount = cooldown / originalCooldownValue;
        }
        else if (!this.parentButton.enabled)
        {
            this.parentButton.enabled = true;
            this.Ready = true;
        }
    }

    public void StartCooldown(float cooldown)
    {
        this.parentButton.enabled = false;
        this.Ready = false;
        this.cooldown = cooldown;
        this.originalCooldownValue = cooldown;
    }

    public void RestartCooldown()
    {
        this.parentButton.enabled = false;
        this.Ready = false;
        this.cooldown = this.originalCooldownValue;
    }
}
