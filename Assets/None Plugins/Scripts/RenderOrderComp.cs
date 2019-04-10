using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class RenderOrderComp : MonoBehaviour
{
	public int orderInLayer;

	private Renderer smr;
	public Renderer CachedRenderer
	{
		get
		{
			if (!smr)
			{
				smr = GetComponent<Renderer>();
			}
			return smr;
		}
	}

	private Anima2D.SpriteMeshInstance smi;
	public Anima2D.SpriteMeshInstance CachedSpriteMeshInstance
	{
		get
		{
			if (!smi)
			{
				smi = GetComponent<Anima2D.SpriteMeshInstance>();
			}
			return smi;
		}
	}
	public bool HasSpriteMesh
	{
		get
		{
			return (CachedSpriteMeshInstance != null);
		}
	}
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

    

    // Update is called once per frame
    void OnWillRenderObject()
    {
		if (CachedRenderer != null)
		{
			CachedRenderer.sortingOrder = orderInLayer;
		}
		if (HasSpriteMesh)
		{
			CachedSpriteMeshInstance.sortingOrder = orderInLayer;
		}
    }

	private void Update()
	{
		CachedRenderer.sortingOrder = orderInLayer;
		if (HasSpriteMesh)
		{
			CachedSpriteMeshInstance.sortingOrder = orderInLayer;
		}
	}
}
