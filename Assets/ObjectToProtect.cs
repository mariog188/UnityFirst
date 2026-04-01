using UnityEngine;

public class ObjectToProtect : Entity
{
    [Header("Details")]
    [SerializeField] private Transform player;

    protected override void Update()
    {
        HandleFLip();
    }

    protected override void HandleFLip()
    {
        if (player.transform.position.x > transform.position.x && !facingright)
        {
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && facingright)
        {
            Flip();
        }
    }
}
