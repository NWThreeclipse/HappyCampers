using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool itemIsPickedUp = false;

    public enum ItemType
    {
        Cozy,
        Spooky,
        Educational,
        Zen,
        Funny,
        Glamp,
        Survivalist,
        Romantic,
        Nature,
        Art,
        Celebration,
        Corporate
    }

    protected ItemType currentItemType;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; 
        }   
    }
    public bool ItemIsPickedUp
    {
        get => itemIsPickedUp;
        set => itemIsPickedUp = value;
    }

    public virtual void ItemIsHeld(Transform player)
    {
        Debug.Log($"{name} got picked up ");
        itemIsPickedUp = true;
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic; 
        transform.SetParent(player);

        transform.localPosition = Vector3.zero;
    }

    public void ItemIsDropped()
    {
        // Unparent the item from the player
        Debug.Log("Dropping item");
        transform.SetParent(null);

        // Reset the state
        ItemIsPickedUp = false;

        // Restore physics (if applicable) and give it a slight push
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Re-enable physics simulation
            // Optional: Give the item a slight force to push it away from the player
            Vector2 dropDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), 0.5f).normalized;
            float dropForce = 5f;
            rb.AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }

        // Optional: Re-enable the collider if you disabled it on pickup
        // Collider2D collider = GetComponent<Collider2D>();
        // if (collider != null) collider.enabled = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Other collider {other.name}");
        if(other.name == "Fire")
        {
            Debug.Log($"Item: {name} Generate Points for {currentItemType} : 10");
        }
    }
    
}
