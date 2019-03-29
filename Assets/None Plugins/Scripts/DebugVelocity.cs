using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugVelocity : MonoBehaviour
{
	Text text;
	public EntityControllerComp ec;
    // Start is called before the first frame update
    void Start()
    {
		text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		text.text = (ec.Velocity - ec.TargetVelocity).ToString();
    }
}
