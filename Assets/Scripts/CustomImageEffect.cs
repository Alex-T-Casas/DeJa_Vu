using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class CustomImageEffect : MonoBehaviour {

	public Material _material;
	

	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Debug.Log("Image render finished");
		if (_material != null) {
			Graphics.Blit(src, dest, _material);
		}
	}
}