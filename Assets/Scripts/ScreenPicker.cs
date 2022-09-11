using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPicker : MonoBehaviour
{
	private void Update()
	{
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit,LayerMask.GetMask("Environment")))
        {
            transform.position = hit.point;
        }
    }
}
