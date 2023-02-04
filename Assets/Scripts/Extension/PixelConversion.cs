using UnityEngine;

// Holds functions used for position conversion between world values and pixel values.
public static class PixelConversion {
	
	// Takes distance measured in pixels and divides it by PPU value.
	public static float ConvertPixelDistanceToWorldDistance(int pixelDist) {
		float worldDist = (float)pixelDist / ProjectSettings.pixelPerUnit;
		return worldDist;
	}
	
	public static float ConvertSubpixelDistanceToWorldDistance(float subpixelDist) {
		float worldDist = subpixelDist / ProjectSettings.pixelPerUnit;
		return worldDist;
	}
	
	// Takes distance measured in world units, multiplies it by PPU value and rounds it to nearest integer.
	public static int ConvertWorldDistanceToPixelDistance(float worldDist) {
		int pixelDist = Mathf.RoundToInt(worldDist * ProjectSettings.pixelPerUnit);
		return pixelDist;
	}
	
	// Takes world position, multiplies it by PPU value and then snaps it to pixel grid.
	public static Vector2 ConvertWorldPositionToPixelPosition(Vector3 worldPos) {
		Vector3 multipliedWorldPos = worldPos * ProjectSettings.pixelPerUnit;
		Vector2 pixelPos = new Vector3(Mathf.Round(multipliedWorldPos.x), Mathf.Round(multipliedWorldPos.y));
		return pixelPos;
	}
	
	// Takes pixel position and divides it by PPU value to receive world position.
	public static Vector3 ConvertPixelPositionToWorldPosition(Vector3 pixelPos) {
		Vector3 worldPos = pixelPos / ProjectSettings.pixelPerUnit;
		return worldPos;
	}
	
	public static Vector3 ConvertPixelPositionToWorldPosition(Vector2 pixelPos) {
		Vector3 worldPos = (Vector3)(pixelPos / ProjectSettings.pixelPerUnit);
		return worldPos;
	}
	
	// Takes world position, snaps it to pixel grid and returns it back.
	public static Vector3 SnapWorldPositionToPixelGrid(Vector3 worldPos) {
		Vector3 snappedPos = ConvertPixelPositionToWorldPosition(ConvertWorldPositionToPixelPosition(worldPos));
		return snappedPos;
	}
}
