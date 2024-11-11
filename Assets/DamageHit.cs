using UnityEngine;

public class DamageHit : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10; // Set the default damage amount in the inspector

    public int GetDamageAmount()
    {
        return damageAmount;
    }
}
