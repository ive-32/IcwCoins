using UnityEngine;

public class IcwArrow : MonoBehaviour
{
    static int serialnum = 0;
    public GameObject ArrowOdd;
    public GameObject ArrowEven;
    public GameObject Dot;

    private void Awake()
    {
        ArrowOdd.SetActive(serialnum % 2 == 0);
        ArrowEven.SetActive(serialnum % 2 == 1);
        serialnum++;    
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        this.transform.parent.TryGetComponent<IRoute>(out IRoute thisRoute);
        if (thisRoute == null || !thisRoute.inProgress) return;
        collision.gameObject.TryGetComponent<IWalker>(out IWalker otherObject);
        if (otherObject == thisRoute.walker && Vector3.Distance(otherObject.Position, transform.position) < 0.5f )
        {
            Color completecolor = new Color(0, 0, 0, 0.6f);
            ArrowOdd.GetComponent<SpriteRenderer>().color = completecolor;
            ArrowEven.GetComponent<SpriteRenderer>().color = completecolor;
            Dot.GetComponent<SpriteRenderer>().color = completecolor;
        }

    }
}
