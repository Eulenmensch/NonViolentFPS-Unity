﻿using UnityEngine;
using System.Collections;
 
public class ObjectDragger : MonoBehaviour 
{
 
	private Vector3 screenPoint;
	private Vector3 offset;

	private void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	private void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		transform.position = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
	}

 
 }