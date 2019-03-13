using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveHandler))]
[RequireComponent(typeof(Targeter))]
[RequireComponent(typeof(EntityController))]
public class GruntComp : MonoBehaviour {

	private MoveHandler moveHandler;
	private Targeter targeter;
	private EntityControllerComp entityController;
	// Use this for initialization
	void Start () {
		moveHandler = GetComponent<MoveHandler>();
		targeter = GetComponent<Targeter>();
		entityController = GetComponent<EntityControllerComp>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
