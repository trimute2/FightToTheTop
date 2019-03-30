using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class RenderOrderComp : MonoBehaviour
{
	public int orderInLayer;
	private SkinnedMeshRenderer smr;

	private Anima2D.SpriteMeshInstance smi;
	private bool hasSMI;
	private int characterDrawOrder = 0;
	public int CharacterDrawOrder
	{
		get
		{
			return characterDrawOrder;
		}
		set
		{
			characterDrawOrder = value;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		smr = GetComponent<SkinnedMeshRenderer>();
		smi = GetComponent<Anima2D.SpriteMeshInstance>();
		hasSMI = (smi != null);
    }

    // Update is called once per frame
    void OnWillRenderObject()
    {
		smr.sortingOrder = orderInLayer;
		if (hasSMI)
		{
			smi.sortingOrder = orderInLayer;
		}
    }

	private void Update()
	{
		smr.sortingOrder = orderInLayer;
		if (hasSMI)
		{
			smi.sortingOrder = orderInLayer;
		}
	}
}
