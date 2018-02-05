using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(CustomGradient))]
public class GradientDrawer : PropertyDrawer
{


	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return 30f;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		Event e = Event.current;

		CustomGradient gradient = (CustomGradient)fieldInfo.GetValue(property.serializedObject.targetObject);
		float labelWidth = GUI.skin.label.CalcSize(label).x + 5;
		Rect textureRect = new Rect(position.x + labelWidth, position.y, position.width - labelWidth, position.height);

		if(e.type == EventType.Repaint)
		{
			GUI.Label(position, label);

			GUIStyle gradientStyle = new GUIStyle();
			gradientStyle.normal.background = gradient.GetTexture((int)position.width);
			GUI.Label(textureRect, GUIContent.none, gradientStyle);
		}
		else
		{
			if(e.type == EventType.MouseDown && e.button == 0)
			{
				if(textureRect.Contains(e.mousePosition))
				{
					GradientEditor window =  EditorWindow.GetWindow<GradientEditor>();
					window.SetGradient(gradient);
				}
			}
		}
	}


}






