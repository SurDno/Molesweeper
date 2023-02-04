using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class CustomTransform : Editor {
	
	public enum Rotations {
		Down = 0,
		Right = 1,
		Up = 2,
		Left = 3
	}
	public Rotations Rotation;

    public override void OnInspectorGUI() {
		// Show a warning message when trying to edit multiple objects at once.
		Object[] allSelectedObjects = targets;
		
		if (allSelectedObjects.Length > 1) {
			EditorGUILayout.HelpBox("Cannot modify the transform values of several elements at once!", MessageType.Warning);
			EditorGUILayout.HelpBox("You can still move, rotate and scale them in editor while maintaining pixel perfectness.", MessageType.Info);
			SnapTransformValuesOfMultipleElements();
			CheckSpecialCases();
			return;
		}
		
		Transform myTarget = (Transform)target;

		// All objects but camera lack Z axis.
		EditorGUILayout.LabelField("Position in pixels");
		Vector2 pos = EditorGUILayout.Vector2IntField("", Vector2Int.RoundToInt(myTarget.position * ProjectSettings.pixelPerUnit));
		myTarget.position = pos / ProjectSettings.pixelPerUnit;
			
		// All objects cannot be rotated by X and Y.
		// Cameras, Tilemaps, Grids can't have their scale and rotation changed.
		if(!myTarget.gameObject.GetComponent<Camera>() && !myTarget.gameObject.GetComponent<Tilemap>() && !myTarget.gameObject.GetComponent<Grid>()) {
			
			
			EditorGUILayout.LabelField("Rotation");
			// By default, all objects can be rotated only by 90 degrees.
			Rotation = (Rotations)EditorGUILayout.EnumPopup("", (Rotations)(myTarget.rotation.eulerAngles.z / 90));
			// Makes the rotation not smooth but instant.
			myTarget.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Round((int)Rotation * 90.0f));
		
			EditorGUILayout.LabelField("Scale");
			// All objects can only use integer scale to avoid pixel splitting, and their height always equals their width.
			int scale = EditorGUILayout.IntSlider((int)myTarget.localScale.x, 1, 6);
			myTarget.localScale = new Vector2((float)scale, (float)scale);	
		}
		
		CheckSpecialCases();
    }	
	
	void SnapTransformValuesOfMultipleElements() {
		Object[] allSelectedObjects = targets;
		
		foreach (Object selectedObject in allSelectedObjects) {
			Transform individualTransform = (Transform)selectedObject;
			
			// Snap position.
			individualTransform.position = (Vector2)(Vector2Int.RoundToInt(individualTransform.position * ProjectSettings.pixelPerUnit)) / ProjectSettings.pixelPerUnit;
			
			// Snap rotation.
			individualTransform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Round((individualTransform.rotation.eulerAngles.z / 90) * 90.0f));
		}
	}
	
	void CheckSpecialCases() {
		Object[] allSelectedObjects = targets;
		
		foreach (Object selectedObject in allSelectedObjects) {
			Transform individualTransform = (Transform)selectedObject;
			
			// Technical objects othen than camera can't have any of their values changed.
			if(individualTransform.gameObject.GetComponent<Tilemap>() ||
			individualTransform.gameObject.GetComponent<Grid>()) {
				individualTransform.position = Vector3.zero;
				individualTransform.rotation = Quaternion.Euler(Vector3.zero);
				individualTransform.localScale = Vector2.one;
			}
			
			// Camera is always can actually move but cannot be rotated or scaled.
			if(individualTransform.gameObject.GetComponent<Camera>())  {
				individualTransform.position = new Vector3(individualTransform.position.x, individualTransform.position.y, 0);
				individualTransform.rotation = Quaternion.Euler(Vector3.zero);
				individualTransform.localScale = Vector2.one;
			}
			
		}
	}
}
