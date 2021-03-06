using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public static List<Explosive> Explosives;
    [SerializeField] private float power;
    [SerializeField] private float powerPlayer;
    [SerializeField] private float radius = 20f;

    [SerializeField] private GameObject explosionCircle;

    /// <summary>
    ///     The layers which this explosion should affect.
    /// </summary>
    [SerializeField] private LayerMask layerMask;

    private Rigidbody[] _bodyParts;
    private CharacterJoint[] _joints;

    private void Awake()
    {
        _bodyParts = GetComponentsInChildren<Rigidbody>();
        _joints = GetComponentsInChildren<CharacterJoint>();
        if (Explosives == null) Explosives = new List<Explosive>();
        Explosives.Add(this);
    }

    public void Explode()
    {
        foreach (var joint in _joints) Destroy(joint);

        foreach (var part in _bodyParts)
            part.AddForce((part.transform.position - transform.position).normalized * power, ForceMode.Force);
        
        var hits = Physics.OverlapSphere(transform.position, radius, layerMask);
        var circle = Instantiate(explosionCircle, transform.position, Quaternion.identity);
        circle.transform.localScale *= radius * 2;
        
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Corpse"))
                hit.attachedRigidbody.AddForce((hit.transform.position - transform.position).normalized * power,
                    ForceMode.Impulse);

            if (hit.CompareTag("Player"))
            {
                var controller = hit.GetComponent<ThirdPersonController>();
                controller.SetKnockback(hit.transform.position - transform.position, powerPlayer);
            }
        }

        Explosives.Remove(this);
        StartCoroutine(Remove());
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(0.5f);
        
        gameObject.SetActive(false);
    }
}