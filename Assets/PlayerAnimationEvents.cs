using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void DisableJumpAndMovement() =>
        player.EnableJumpAndMovement(false);

    private void EnableJumpAndMovement() =>
        player.EnableJumpAndMovement(true);

}
