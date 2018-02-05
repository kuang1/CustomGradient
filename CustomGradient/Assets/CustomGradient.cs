using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CustomGradient
{
	Texture2D _tex;

	[SerializeField]
	List<ColorKey> keys = new List<ColorKey>();


	public Color Evaluate(float t)
	{
		if(keys.Count == 0)
			return Color.white;

		ColorKey keyLeft = keys[0];
		ColorKey keyRight = keys[keys.Count - 1];

		for(int i = 0; i < keys.Count - 1; i ++)
		{
			if(keys[i].time <= t && keys[i + 1].time >= t)
			{
				keyLeft = keys[i];
				keyRight = keys[i + 1];
				break;
			}
		}

		float tBlend = Mathf.InverseLerp(keyLeft.time, keyRight.time, t);
		return Color.Lerp(keyLeft.color, keyRight.color, tBlend);
	}

	public int AddKey(Color color, float time)
	{
		ColorKey newKey = new ColorKey(color, time);
		for(int i = 0; i < keys.Count; i ++)
		{
			if(newKey.time < keys[i].time)
			{
				keys.Insert(i, newKey);
				return i;
			}
		}
		keys.Add(newKey);
		return keys.Count - 1;
	}

	public void RemoveKey(int index)
	{
		if(keys.Count >= 2)
		{
			keys.RemoveAt(index);
		}
	}

	public int UpdateKeyTime(int index, float time)
	{
		Color col = keys[index].color;
		RemoveKey(index);
		return AddKey(col, time);
	}

	public void UpdateKeyColor(int index, Color color)
	{
		keys[index] = new ColorKey(color, keys[index].time);
	}

	public int KeyCount
	{
		get { return keys.Count; }
	}

	public ColorKey GetKey(int i)
	{
		return keys[i];
	}

	public Texture2D GetTexture(int width)
	{
		if(_tex == null || _tex.width != width)
		{
			_tex = new Texture2D(width, 1);
		}
		FillTexture(_tex);
		return _tex;
	}

	private void FillTexture(Texture2D texture)
	{
		Color[] colors = new Color[texture.width];
		for(int i = 0; i < colors.Length; i ++)
		{
			colors[i] = Evaluate((float)i / (colors.Length - 1));
		}
		texture.SetPixels(colors);
		texture.Apply();
	}



	[System.Serializable]
	public struct ColorKey
	{
		[SerializeField]
		Color _color;
		[SerializeField]
		float _time;


		public ColorKey(Color color, float time)
		{
			this._color = color;
			this._time = time;
		}


		public Color color
		{
			get { return _color; }
		}
		public float time
		{
			get { return _time; }
		}


	}

}






