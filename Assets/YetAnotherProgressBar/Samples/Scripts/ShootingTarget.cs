using UnityEngine;
using System.Collections;
using System;

public class ShootingTarget : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rbody;
    private Coroutine rotateCoroutine;
    private Vector3 scaleIncrement = new Vector3(0.1f, 0.1f, 0.1f);
    
    void Start()
    {
        rotateCoroutine = StartCoroutine(Rotate());
        rbody.transform.localScale = Vector3.zero;
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            if (rbody != null)
            {
                rbody.AddRelativeTorque(new Vector3(1, 2, 3), ForceMode.Impulse);
                if (rbody.transform.localScale.x < 1)
                {
                    rbody.transform.localScale += scaleIncrement;
                }
            }
            //transform.Rotate(1, 2, 3);
            yield return null;
        }
    }

    public void Hit(Vector3 hitDirection)
    {
        if (rbody != null)
        {
            //rbody.isKinematic = false;
            rbody.useGravity = true;
            rbody.AddRelativeTorque(new Vector3(1, 2, 3), ForceMode.Impulse);
            rbody.AddForce(hitDirection * 80, ForceMode.Impulse);
        }
        StopCoroutine(rotateCoroutine);
    }

    void OnCollisionEnter(Collision collision)
    {
        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
        //Debug.Log("OnCollisionEnter");
        if (collision.gameObject.layer == TargetInstancer.GroundLayer)
        {
            StartCoroutine(RemoveRigidBody());
        }
    }

    private IEnumerator RemoveRigidBody()
    {
        yield return new WaitForSeconds(5);
        Destroy(rbody);
    }
}
