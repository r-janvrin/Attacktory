using Unity.VisualScripting;
using UnityEngine;

public class conveyorResourceController : MonoBehaviour
{
    public sbyte resourceType;
    private Vector2 target;
    public float speed;


    public void setup(sbyte resType, Vector3 targetPos, float speedValue)
    {
        resourceType = resType;
        target = targetPos;
        speed = speedValue;
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

    public void delete()
    {
        GameObject.Destroy(transform.gameObject);
    }
}
