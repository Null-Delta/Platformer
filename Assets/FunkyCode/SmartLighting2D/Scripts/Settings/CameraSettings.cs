using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif



[System.Serializable]
public class CameraMaterials {
	public Material[] materials = new Material[1];

	//public static readonly int _Radius = Shader.PropertyToID("_Radius");
	// public static readonly int _BackgroundTexture = Shader.PropertyToID("_SuperBlurTexture");
    // blurMaterial.SetFloat(Uniforms._Radius, radius);
	
	public static void ClearMaterial(Material material) {
		material.SetVector("_Cam1_Rect", new Vector4(0, 0, 0, 0));
		material.SetVector("_Cam2_Rect", new Vector4(0, 0, 0, 0));
	}

	public static void SetMaterial(int id, Material material, Camera camera, LightTexture lightTexture) {
		float ratio = (float)camera.pixelRect.width / camera.pixelRect.height;

		float x = camera.transform.position.x;
		float y = camera.transform.position.y;

		// z = size x ; w = size y
		float z = camera.orthographicSize * ratio * 2;
		float w = camera.orthographicSize * 2;

		float rotation = camera.transform.eulerAngles.z * Mathf.Deg2Rad;

		switch(id) {
			case 1:
				material.SetTexture("_Cam1_Texture", lightTexture.renderTexture);

				material.SetVector("_Cam1_Rect", new Vector4(x, y, z, w));

				material.SetFloat("_Cam1_Rotation", rotation);
			break;

			case 2:
				material.SetTexture("_Cam2_Texture", lightTexture.renderTexture);

				material.SetVector("_Cam2_Rect", new Vector4(x, y, z, w));

				material.SetFloat("_Cam2_Rotation", rotation);
			break;
		}
		
	}

	public void Add(Material material) {
		foreach(Material m in  materials) {
			if (m == material) {
				Debug.Log("Lighting Manager 2D: Failed to add material (material already added!");
				return;
			}
		}

		for(int i = 0 ; i < materials.Length; i++) {
			if (materials[i] != null) {
				continue;
			}
			materials[i] = material;

			return;
		}

		System.Array.Resize(ref materials, materials.Length + 1);

		materials[materials.Length - 1] = material;
	}

	public void Remove(Material material) {
		for(int i = 0 ; i < materials.Length; i++) {
			if (materials[i] != material) {
				continue;
			}
			materials[i] = null;

			return;
		}

		Debug.LogWarning("Lighting Manager 2D: Removing material that does not exist");
	}

	public void SetCamera(int id) {

	}

}

[System.Serializable]
public struct CameraBufferPreset {
	public enum RenderMode { Draw, Hidden, Disabled }
	public enum RenderShader { Multiply, Additive, Custom };
	public enum RenderLayerType { Default, Custom };

	public RenderLayerType renderLayerType;
	public RenderMode renderMode;
	public RenderShader renderShader;
	public Material customMaterial;

	public Material customMaterialInstance;

	public CameraMaterials materials;

	public int renderLayerId;

	public int id;

	public int bufferID;

	public CameraBufferPreset(int id = 0) {
		Debug.Log("Creating Buffer Preset! " + id);

		this.id = id;

		this.bufferID = 0;

		this.renderMode = RenderMode.Draw;

		this.renderShader = RenderShader.Multiply;

		this.renderLayerType = RenderLayerType.Default;

		this.customMaterial = null;

		this.customMaterialInstance = null;

		this.renderLayerId = 0;
		
		this.materials = new CameraMaterials();
	}

	
	public CameraMaterials GetMaterials() {
		if (materials == null) {
			materials = new CameraMaterials();
		}

		return(materials);
	}

	public Material GetMaterial() {
		if (customMaterialInstance == null) {
			if (customMaterial != null) {
				customMaterialInstance = new Material(customMaterial);
			}
		}

		return(customMaterialInstance);
	}
}

[System.Serializable]
public struct CameraSettings {
	public enum CameraType { MainCamera, Custom, SceneView };

	public int id;

	[SerializeField]
	private CameraBufferPreset[] bufferPresets;

	public CameraBufferPreset[] BufferPresets {
		get {
			if (bufferPresets == null) {
				bufferPresets = new CameraBufferPreset[1];
			}

			return(bufferPresets);
		}

		set {
			bufferPresets = value;
		}
	}

	public CameraBufferPreset GetBufferIndex(int index) {
		CameraBufferPreset buffer = bufferPresets[index];
		buffer.id = index;
		return(buffer);
	}

	public CameraType cameraType;
	public Camera customCamera;

	public string GetTypeName() {
		switch(cameraType) {
			case CameraType.SceneView:
				return("Scene View");

			case CameraType.MainCamera:
				return("Main Camera Tag");

			case CameraType.Custom:
				return("Custom");

			default:
				return("Unknown");
		}
	}

	public int GetLayerId(int bufferId) {
		CameraBufferPreset bufferPreset = GetBufferIndex(bufferId);

		if (bufferPreset.renderLayerType == CameraBufferPreset.RenderLayerType.Custom) {
			return(bufferPreset.renderLayerId);
		} else {
			Camera camera = GetCamera();

			if (camera != null && cameraType == CameraType.SceneView) {
				return(Lighting2D.ProjectSettings.editorView.sceneViewLayer);
			} else {
				return(Lighting2D.ProjectSettings.editorView.gameViewLayer);
			}
		}
	}

	public CameraSettings(int id = 0) {
		Debug.Log("Creating Camera setting!" + id);
		this.id = id;

		cameraType = CameraType.MainCamera;

		customCamera = null;
		
		bufferPresets = new CameraBufferPreset[1];
	}

	public Camera GetCamera() {
		Camera camera = null;
		switch(cameraType) {
			case CameraType.MainCamera:
				camera = Camera.main;

				if (camera != null) {
					if (camera.orthographic == false) {
						return(null);
					}
				}

				return(Camera.main);

			case CameraType.Custom:
				camera = customCamera;

				if (camera != null) {
					if (camera.orthographic == false) {
						return(null);
					}
				}

				return(customCamera);


            case CameraType.SceneView:
			
				#if UNITY_EDITOR
					SceneView sceneView = SceneView.lastActiveSceneView;

					if (sceneView != null) {
						camera = sceneView.camera;

						#if UNITY_2019_1_OR_NEWER
						
							if (SceneView.lastActiveSceneView.sceneLighting == false) {
								camera = null;
							}

						#else
						
							if (SceneView.lastActiveSceneView.m_SceneLighting == false) {
								camera = null;
							}

						#endif
					}
	
					if (camera != null && camera.orthographic == false) {
						camera = null;
					}

					if (camera != null) {
						if (camera.orthographic == false) {
							return(null);
						}
					}

					return(camera);

				#else
					return(null);

				#endif
				
		}

		return(null);
	}

	/*
	public bool Equals(CameraSettings obj) {
        return this.bufferID == obj.bufferID && this.customCamera == obj.customCamera && this.cameraType == obj.cameraType;
    }*/

	public override int GetHashCode() {
        return this.GetHashCode();
    }
}