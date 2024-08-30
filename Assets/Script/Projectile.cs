using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] private float existTime;
    [SerializeField] private bool penetrating;
    [SerializeField] private string penetratingTag;
    public int damage = 20;
    public int hitBackLevel;

    public bool rotate = false;
    public float rotateAmount = 45;
    public float speed;

	//public float fireRate;
	public GameObject muzzlePrefab;
	public GameObject hitPrefab;

    private Vector3 startPos;
	private Vector3 offset;
	private Rigidbody rb;
    private GameObject player;

    public string shooter;

	void Start () {
        Destroy(this.gameObject, existTime);
        player = GameObject.FindGameObjectWithTag("Player");
        startPos = transform.position;
        rb = GetComponent <Rigidbody> ();
			
		if (muzzlePrefab != null) {
			var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = gameObject.transform.forward + offset;
			var ps = muzzleVFX.GetComponent<ParticleSystem>();
			if (ps != null)
				Destroy (muzzleVFX, ps.main.duration);
			else {
				var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
			}
		}
	}

	void FixedUpdate () {
        if (rotate)
            transform.Rotate(0, 0, rotateAmount, Space.Self);
        if (speed != 0 && rb != null)
			rb.position += (transform.forward + offset) * (speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag != "Bullet" && co.gameObject.tag != shooter && !co.isTrigger)
        {            
            Quaternion rot = Quaternion.identity;
            Vector3 pos = this.transform.position;
            if (co.transform.tag == "Enemy")
            {
                co.transform.GetComponent<Enemy>().GetHit(damage);
            }
            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot) as GameObject;                   

                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                    Destroy(hitVFX, ps.main.duration);
            }
            if (penetrating && penetratingTag == co.gameObject.tag)
            {
                return;
            }
            StartCoroutine(DestroyParticle(0f));
        }
    }


    public IEnumerator DestroyParticle (float waitTime) {

		if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = new List<Transform> ();

			foreach (Transform t in transform.GetChild(0).transform) {
				tList.Add (t);
			}		

			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				for (int i = 0; i < tList.Count; i++) {
					tList[i].localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
