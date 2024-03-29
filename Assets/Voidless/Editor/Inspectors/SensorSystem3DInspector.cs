﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(SensorSystem3D))]
public class SensorSystem3DInspector : Editor
{
	private const float SPACE_INDENT = 35.0f; 	/// <summary>Indent's Space.</summary>

	private SensorSystem3D sensorsystem3D; 		/// <summary>Inspector's Target.</summary>
	private SerializedProperty subsystems; 		/// <summary>Subsystem's Serialized Property.</summary>
	private SerializedProperty layerMask; 		/// <summary>LayerMask's Serialized Property.</summary>
	private SerializedProperty relativeOrigin; 	/// <summary>Relative Origin's Serialized Property.</summary>
	private SerializedProperty sensorsData; 	/// <summary>Sensor Data's Serialized Property.</summary>
	private SerializedProperty direction; 		/// <summary>Direction's Serialized Property.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		sensorsystem3D = target as SensorSystem3D;
		subsystems = serializedObject.FindProperty("_subsystems");
		EditorUtility.SetDirty(sensorsystem3D);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawSubsystems();

		serializedObject.ApplyModifiedProperties();
	}

	/// <summary>Draws all subsystems with their respective sensors' data.</summary>
	private void DrawSubsystems()
	{
		EditorGUILayout.Space();
		VEditorGUILayout.ShowSerializedPropertySizeConfiguration(ref subsystems, "Subsystems' ");
		EditorGUILayout.Space();

		if(sensorsystem3D.subsystems != null)
		for(int i = 0; i < sensorsystem3D.subsystems.Length; i++)
		{
			string subsystemLabel = "Subsystem " + i;
			SensorSubsystem3D subsystem = sensorsystem3D.subsystems[i];
			layerMask = subsystems.GetArrayElementAtIndex(i).FindPropertyRelative("_layerMask");
			//relativeOrigin = subsystems.GetArrayElementAtIndex(i).FindPropertyRelative("_relativeOrigin");

			EditorGUILayout.LabelField(subsystemLabel + ": ");
			EditorGUILayout.Space();

			subsystem.color = EditorGUILayout.ColorField("Gizmos' Color", subsystem.color);
			//subsystem.relativeTo = (RelativeTo)EditorGUILayout.EnumPopup("Relative To: ", subsystem.relativeTo);
			//EvaluateComponents(i);
			EditorGUILayout.PropertyField(layerMask);
			//EditorGUILayout.PropertyField(relativeOrigin);
			//subsystem.originDistance = EditorGUILayout.FloatField("Origin's Distance: ", subsystem.originDistance);

			sensorsData = subsystems.GetArrayElementAtIndex(i).FindPropertyRelative("_sensorsData");
			EditorGUILayout.Space();
			VEditorGUILayout.ShowSerializedPropertySizeConfiguration(ref sensorsData, subsystemLabel + " Sensors' Data ");
			VEditorGUILayout.Spaces(2);

			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(SPACE_INDENT);
			EditorGUILayout.BeginVertical();
			for(int j = 0; j < subsystem.sensorsData.Length; j++)
			{
				direction = sensorsData.GetArrayElementAtIndex(j).FindPropertyRelative("_direction");

				EditorGUILayout.LabelField("Sensor's Data " + j + ": ");
				EditorGUILayout.Space();
				subsystem.sensorsData[j].sensorType = (SensorType)EditorGUILayout.EnumPopup("Sensor's Type: ", subsystem.sensorsData[j].sensorType);
				subsystem.sensorsData[j].origin = EditorGUILayout.Vector3Field("Origin: ", subsystem.sensorsData[j].origin);
				EditorGUILayout.PropertyField(direction);
				subsystem.sensorsData[j].distance = EditorGUILayout.FloatField("Distance: ", subsystem.sensorsData[j].distance);
				if(subsystem.sensorsData[j].sensorType != SensorType.Ray)
				{
					subsystem.sensorsData[j].dimensions = EditorGUILayout.Vector3Field("Dimensions: ", subsystem.sensorsData[j].dimensions);
					if(subsystem.sensorsData[j].sensorType == SensorType.Sphere || subsystem.sensorsData[j].sensorType == SensorType.Capsule)
					EditorGUILayout.LabelField("Radius Equals: " + subsystem.sensorsData[j].radius);
				}

				VEditorGUILayout.Spaces(2);	
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			VEditorGUILayout.Spaces(3);
		}
	}

	/// <summary>Evaluates if Sensor System has either Renderer or Component reference.</summary>
	/// <param name="_index">Index of the current Subsystem.</param>
	private void EvaluateComponents(int _index)
	{
		SensorSubsystem3D subsystem = sensorsystem3D.subsystems[_index];
		//RelativeTo relativeTo = subsystem.relativeTo;

		/*switch(relativeTo)
		{
			case RelativeTo.RendererBounds:
			if(sensorsystem3D.renderer == null)
			{
				subsystem.relativeTo = RelativeTo.Transform;
				ShowComponentNotFoundWindow("Renderer");
			}
			break;

			case RelativeTo.ColliderBounds:
			if(sensorsystem3D.collider == null)
			{
				subsystem.relativeTo = RelativeTo.Transform;
				ShowComponentNotFoundWindow("Collider");
			}
			break;
		}*/
	}

	/// <summary>Shows a Component Not Found Message Window.</summary>
	/// <param name="_componentName">Name of the Component missing.</param>
	private void ShowComponentNotFoundWindow(string _componentName)
	{
		EditorUtility.DisplayDialog
		(
			"Missing " + _componentName + "!",
			"Sensor System does not contain a " + _componentName + " component. The default value (Relative to Transform) will be set instead.",
			"Ok"
		);
	}
}
}