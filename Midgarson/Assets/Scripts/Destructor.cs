using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        Destroy(other.gameObject);
        Destroy(other.transform.parent.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other){
        Destroy(other.gameObject);
    }
}
