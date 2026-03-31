using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float redColorDuration = 2;
    public float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer = timer - Time.deltaTime;
        
        if(timer < 0 && spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void TakeDamage()
    {
        spriteRenderer.color = Color.red;
        timer = redColorDuration;
    }

    private void TurnWhite()
    {
        spriteRenderer.color = Color.white;
    }
}
