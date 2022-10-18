using UnityEngine;

public class IcwPlayer : MonoBehaviour , IWalker
{
    private Vector3 target;
    private Rigidbody2D rg2d;
    private float playerSpeed;
    public bool IsMoving 
    { 
        get 
        {
            if (rg2d == null) return false;
            return rg2d.velocity != Vector2.zero; 
        } 
    }
    public Vector3 Position { get => this.transform.position; }
    public bool TargetReached { get => Vector3.Distance(target, this.transform.position) < playerSpeed * Time.deltaTime; }

    private void Awake()
    {
        rg2d = this.GetComponent<Rigidbody2D>();
        if (rg2d == null)
        {
            Debug.LogError("Rigid Body 2d not founded on Player Prefab");
            Destroy(this.gameObject);
            return;
        }
        target = this.transform.position;
        playerSpeed = 2.0f;
    }

    private void Update()
    {
        if (TargetReached)
        {   // Если достигнем конечной точки в этом кадре
            // то становимся точно на конечную точку
            this.transform.position = target;
            SetTarget(null);
        }
    }

    public void SetTarget(Vector3? atarget)
    {
        if (atarget != null)
        {
            target = (Vector3)atarget;
            rg2d.velocity = (target - this.transform.position).normalized * playerSpeed;
            float angle = Vector3.SignedAngle(Vector3.right, rg2d.velocity, Vector3.forward);
            this.transform.rotation = Quaternion.identity;
            this.transform.Rotate(new Vector3(0, 0, angle));
        }
        else
        {
            target = this.transform.position;
            rg2d.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            Destroy(this.gameObject);

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coins" 
            && Vector3.Distance(this.transform.position, collision.gameObject.transform.position) < 0.3f)
            Destroy(collision.gameObject);
    }

}
