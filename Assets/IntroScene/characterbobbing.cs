using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBobbing : MonoBehaviour
{
    public float bobAmount = 0.05f;
    public float bobSpeed = 5f;
    private Vector3 originalPos;
    private bool isTalking = false;

    void Start() => originalPos = transform.localPosition;

    void Update()
    {
        if (isTalking)
        {
            transform.localPosition = originalPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void SetTalking(bool talking)
    {
        isTalking = talking;
    }
}
