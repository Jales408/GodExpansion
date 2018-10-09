using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{

    public GameObject Arrow;
    public float distanceSpawn;
    public float timeBtwAttack;
    public float offset;
    public float childLifeTime;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(DelayNextAttack(timeBtwAttack + offset));
    }
    IEnumerator DelayNextAttack(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(Instantiate(Arrow, transform.position + Vector3.left * distanceSpawn, Quaternion.identity),childLifeTime);
        StartCoroutine(DelayNextAttack(timeBtwAttack));
    }
}
