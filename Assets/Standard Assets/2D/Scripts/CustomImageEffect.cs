using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CustomImageEffect : MonoBehaviour
{
	public Material EffectMaterial;
	public List<Vector3> lasers;
	private Camera myCamera;

	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		RenderTexture tmp = RenderTexture.GetTemporary (src.width, src.height);
		Graphics.Blit (src, tmp);
		foreach (Vector3 ray in lasers) {
			RenderTexture tmp2 = RenderTexture.GetTemporary (tmp.width, tmp.height);
			DrawLine (ray.x, ray.y, ray.z);
			Graphics.Blit (tmp, tmp2, EffectMaterial);
			RenderTexture.ReleaseTemporary (tmp);
			tmp = tmp2;
		}
		Graphics.Blit (tmp, dst);
		RenderTexture.ReleaseTemporary (tmp);
		lasers.Clear ();
	}

	public void Start ()
	{
		lasers = new List<Vector3> ();
		myCamera = gameObject.GetComponent<Camera> ();
	}

	public void DrawLine (float x1, float x2, float y)
	{
		Vector3 one = myCamera.WorldToScreenPoint (new Vector3 (x1, y, 0));
		Vector3 two = myCamera.WorldToScreenPoint (new Vector3 (x2, y, 0));
		Vector4 ray = new Vector4 (one.x / myCamera.pixelWidth,
			             one.y / myCamera.pixelHeight,
			             two.x / myCamera.pixelWidth,
			             two.y / myCamera.pixelHeight);
		Shader.SetGlobalVector ("_Ray", ray);
	}
}
