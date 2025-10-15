using UnityEngine;

public class Box : Item 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentItemType = ItemType.Survivalist;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void ItemIsHeld(Transform player)
    {
        base.ItemIsHeld(player);

    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
