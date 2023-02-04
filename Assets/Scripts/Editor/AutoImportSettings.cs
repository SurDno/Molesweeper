using UnityEngine;
using UnityEditor;

// Automatically sets import settings for textures for pixel art, avoiding the blurry distorted look and making all elements have the same PPU value.
public class AutoImportSettings : AssetPostprocessor {
	
	void OnPreprocessTexture() {
			// Initialize texture importer.
            TextureImporter textureImporter = assetImporter as TextureImporter;
			
			// Disable filtering to get sharp pixel art.
			textureImporter.filterMode = FilterMode.Point;
			
			// Disable compression to avoid color changes.
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
			
			// Set same PPU for all textures on import.
			textureImporter.spritePixelsPerUnit = ProjectSettings.pixelPerUnit;
			
			// Set highest possible texture max size to avoid pixel splitting that happens when the value is less than original resolution.	
			textureImporter.maxTextureSize = 16384;
    }
}
