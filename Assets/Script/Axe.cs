using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;
    Vector3 targetPos;
    private GameObject player;
    private bool hitPlayer = false;
    void Start()
    {
        SetPlayer(GameObject.FindGameObjectWithTag("Player"));
        targetPos = Camera.main.transform.position;
        transform.LookAt(targetPos);
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; 
        }
    }

    void Update()
    {
        if(hitPlayer)
        {
            return;
        }
        transform.Rotate(Vector3.left, rotateSpeed * Time.deltaTime, Space.Self);
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < 1)
        {
            HitPlayer();
        }
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
    public void GetHit()
    {
        if (hitPlayer)
        {
            return;
        }
        hitPlayer = true;
        rb.isKinematic = false; 
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); 
        rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        Destroy(this.gameObject, 1f);
    }
    private void HitPlayer()
    {
        player.GetComponent<Player>().GetHit();
        hitPlayer = true;
        Destroy(this.gameObject, 1f);
    }
}
