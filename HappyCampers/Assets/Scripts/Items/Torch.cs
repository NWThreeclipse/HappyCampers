using UnityEngine;

public class Torch : Item 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        currentItemType = ItemType.Nature;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
