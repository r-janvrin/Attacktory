using UnityEngine;

public class conveyorResourceController : MonoBehaviour
{
    private sbyte resourceType;
    private Vector2 target;
    public float speed;


    void setup(sbyte resType)
    {
        resourceType = resType;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void setTarget(Vector2 position)
    {
        target = position;
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    public void setSprite(Sprite sprite)
    {
        transform.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
