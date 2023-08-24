using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class  BundleBuilder
{

	const string kAssetBundlesOutputPath = "AssetBundles";

	[MenuItem ("AssetBundles/Build AssetBundles")]
	static public void BuildAssetBundles ()
	{
		// Choose the output path according to the build target.
		string outputPath = Path.Combine(kAssetBundlesOutputPath,  GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget) );
		if (!Directory.Exists(outputPath) )
			Directory.CreateDirectory (outputPath);

		BuildPipeline.BuildAssetBundles (outputPath, 0, EditorUserBuildSettings.activeBuildTarget);

	}

	#if UNITY_EDITOR
	public static string GetPlatformFolderForAssetBundles(BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "Android";
		case BuildTarget.iOS:
			return "iOS";
		//case BuildTarget.WebPlayer:
		//	return "WebPlayer";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "Windows";
		//case BuildTarget.StandaloneOSXIntel:
		//case BuildTarget.StandaloneOSXIntel64:
		//case BuildTarget.StandaloneOSXUniversal:
		//	return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
		default:
			return null;
		}
	}
	#endif
}
