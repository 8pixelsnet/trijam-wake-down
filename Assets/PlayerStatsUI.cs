using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Image shieldImage;

    [SerializeField] private TextMeshProUGUI lifeText;

    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI projectileText;


    // Update is called once per frame
    void Update()
    {
        lifeText.text = player.Life.ToString();
        shieldImage.gameObject.SetActive(player.HasArmor);
        projectileText.text = player.CurrentProjectiles.ToString();
    }
}
