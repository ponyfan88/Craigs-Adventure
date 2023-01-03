using UnityEngine;

public class DragonStateManager : MonoBehaviour
{
    #region Variables

    Transform player;
    bool followPlayerX = true;
    [SerializeField] private float speed = 5f;

    #endregion

    #region Default Methods

    void Awake()
    {
        player = GameObject.Find("player").transform;
    }

    void Update()
    {
        if (followPlayerX)
        {
            float dist = JMath.Distance(transform.position.x, player.position.x);
            Debug.Log(dist + ", " + speed *Time.deltaTime);
            if (dist > speed * Time.deltaTime)
            {
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            }
            else if (dist < -(speed * Time.deltaTime))
            {
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            }
        }
    }
    #endregion

    #region Custom Methods
    #endregion
}
