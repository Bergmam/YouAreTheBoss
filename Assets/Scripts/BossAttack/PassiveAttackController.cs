using UnityEngine;
using UnityEngine.UI;

public class PassiveAttackController : MonoBehaviour{

    private BossAttack currentAttack;
    private ColorModifier aimColorModifier;

    void Awake() {
        this.aimColorModifier = UnityUtils.RecursiveFind(transform, "Image").GetComponent<ColorModifier>();
    }

    public void SetAttack(int attackNumber){
        
        this.aimColorModifier.Select();
    }
}