using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    [SerializeField]
    public float IncreasingRate = 50f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("incrementa");
            HealthSystem.Instance.RestoreMana(IncreasingRate);
        }

    }


}
