using Unity.VisualScripting;
using UnityEngine;

public class KillerWall : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col){
        Player player = col.gameObject.GetComponent<Player>();
        if (player) {
            player.GameOver();
        }
    }
}
