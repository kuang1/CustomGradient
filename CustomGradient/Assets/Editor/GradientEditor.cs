using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GradientEditor : EditorWindow
{
	CustomGradient gradient;

	const int borderSize = 10;
	const float keyWidth = 10;
	const float keyHeight = 20;

	Rect gradientPreviewRect;
	Rect[] keyRects;
	bool mouseIsDownOverKey;
	int selectedKeyIndex;
	bool needRepaint;


	public void SetGradient(CustomGradient gradient)
	{
		this.gradient = gradient;
	}


	void OnEnable()
	{
		titleContent.text = "GradientEditor";
	}

	void OnGUI()
	{
		Draw();
		HandleInput();

		if(needRepaint)
		{
			needRepaint = false;
			Repaint();
		}
	}


	void Draw()
	{
		gradientPreviewRect = new Rect(borderSize, borderSize, position.width - borderSize*2f, 25f);
		GUI.DrawTexture(gradientPreviewRect, gradient.GetTexture((int)gradientPreviewRect.width));

		keyRects = new Rect[gradient.KeyCount];
		for(int i = 0; i < gradient.KeyCount; i ++)
		{
			CustomGradient.ColorKey key = gradient.GetKey(i);
			Rect keyRect = new Rect(gradientPreviewRect.x + gradientPreviewRect.width * key.time - keyWidth/2f, gradientPreviewRect.yMax + borderSize, keyWidth, keyHeight);

			if(i == selectedKeyIndex)
			{
				EditorGUI.DrawRect(new Rect(keyRect.x - 2, keyRect.y - 2, keyRect.width + 4, keyRect.height + 4), Color.black);
			}
			EditorGUI.DrawRect(keyRect, key.color);
			keyRects[i] = keyRect;
		}

		Rect settingRect = new Rect(borderSize, keyRects[0].yMax + borderSize, position.width - borderSize*2f, position.height);
		GUILayout.BeginArea(settingRect);
		EditorGUI.BeginChangeCheck();
		Color newColor = EditorGUILayout.ColorField(gradient.GetKey(selectedKeyIndex).color);
		if(EditorGUI.EndChangeCheck())
		{
			gradient.UpdateKeyColor(selectedKeyIndex, newColor);
		}
		GUILayout.EndArea();
	}

	void HandleInput()
	{
		Event e = Event.current;
		if(e.type == EventType.MouseDown && e.button == 0)
		{
			for(int i = 0; i < keyRects.Length; i ++)
			{
				if(keyRects[i].Contains(e.mousePosition))
				{
					mouseIsDownOverKey = true;
					selectedKeyIndex = i;
					needRepaint = true;
					break;
				}
			}

			if(!mouseIsDownOverKey)
			{
				Color randomColor = new Color(Random.value, Random.value, Random.value);
				float keyTime = Mathf.InverseLerp(gradientPreviewRect.x, gradientPreviewRect.xMax, e.mousePosition.x);
				selectedKeyIndex = gradient.AddKey(randomColor, keyTime);
				mouseIsDownOverKey = true;
				needRepaint = true;
			}
		}

		if(e.type == EventType.MouseUp && e.button == 0)
		{
			mouseIsDownOverKey = false;
		}

		if(mouseIsDownOverKey && e.type == EventType.MouseDrag && e.button == 0)
		{
			float keyTime = Mathf.InverseLerp(gradientPreviewRect.x, gradientPreviewRect.xMax, e.mousePosition.x);
			selectedKeyIndex = gradient.UpdateKeyTime(selectedKeyIndex, keyTime);
			needRepaint = true;
		}

		if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Backspace)
		{
			gradient.RemoveKey(selectedKeyIndex);
			if(selectedKeyIndex >= gradient.KeyCount)
			{
				selectedKeyIndex --;
			}
			needRepaint = true;
		}
	}



}






