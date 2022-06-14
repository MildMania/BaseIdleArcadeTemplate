using System.Globalization;
using UnityEngine;

public static class GameUtilities
{
	public static Vector3 GetWorldToScreenSpace(Vector3 worldPos, Camera mainCamera, RectTransform rectTransform)
	{
		Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPos);
		screenPoint.z = 0;

		Vector2 screenPos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, mainCamera,
			out screenPos))
		{
			return screenPos;
		}

		return screenPoint;
	}

	public static string FormatNumber(this decimal num)
	{
		if (num > 999999999 || num < -999999999)
		{
			return num.ToString("0,,,.####B", CultureInfo.InvariantCulture);
		}

		if (num > 999999 || num < -999999)
		{
			return num.ToString("0,,.###M", CultureInfo.InvariantCulture);
		}

		if (num > 999 || num < -999)
		{
			return num.ToString("0,.##K", CultureInfo.InvariantCulture);
		}

		return num.ToString(CultureInfo.InvariantCulture);
	}
}