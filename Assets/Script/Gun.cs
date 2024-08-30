using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletGO;
    [SerializeField] private Transform shotTrans;
    [SerializeField] private float force;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float floatSpeed;
    [SerializeField] private float maxFloatDis;
    [SerializeField] private AudioClip shotSE;
    private float currentMoveDis;

    private XRGrabInteractable grabScripts;
    private bool isGrabed = false;
    // Start is called before the first frame update
    private void Start()
    {
        grabScripts = GetComponent<XRGrabInteractable>();
        grabScripts.activated.AddListener(GrabHandler);
    }
    private void Update()
    {
        Rotate();
        Float();
    }
    public void ShotBullet()
    {
        Debug.Log("shot"!);
        GameObject bullet = Instantiate(bulletGO, shotTrans.position, shotTrans.rotation);
        AudioSource.PlayClipAtPoint(shotSE, this.transform.position);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * force, ForceMode.Impulse);
    }   
    private void GrabHandler(ActivateEventArgs a)
    {
        ShotBullet();
    }
    private void Rotate()
    {
        if (isGrabed) { return; }
        Vector3 temp = this.transform.rotation.eulerAngles;
        temp.y += Time.deltaTime * rotateSpeed;
        this.transform.rotation = Quaternion.Euler(temp);
    }
    private bool isMovingUp = true;
    private float floatDis = 0;
    private void Float()
    {
        if (isGrabed) { return; }
        if (isMovingUp)
        {
            if (floatDis < maxFloatDis)
            {
                floatDis += Time.deltaTime * floatSpeed;
                Vector3 temp = this.transform.position;
                temp.y += Time.deltaTime * floatSpeed;
                this.transform.position = temp;

            }
            else
            {
                isMovingUp = !isMovingUp;
                floatDis = 0;
            }
        }
        else
        {
            if (floatDis < maxFloatDis)
            {
                floatDis += Time.deltaTime * floatSpeed;
                Vector3 temp = this.transform.position;
                temp.y -= Time.deltaTime * floatSpeed;
                this.transform.position = temp;
            }
            else
            {
                isMovingUp = !isMovingUp;
                floatDis = 0;
            }
        }
    }
    public void OnGrab()
    {
        foreach(Collider c in colliders)
        {
            c.enabled = false;
        }
        isGrabed = true;
    }
    public void OnThrow()
    {
        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }
        GetComponent<Rigidbody>().useGravity = true;
    }
}
