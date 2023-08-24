using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csBundleLoader : MonoBehaviour {


	public static string GetRelativePath()
	{
		if (Application.isEditor)
			#if UNITY_EDITOR//_OSX
			return System.Environment.CurrentDirectory.Replace ("\\", "/"); // Use the build output folder directly.
			#else
			return "file://" +  System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.
			#endif
		//else if (Application.isWebPlayer)
		//	return System.IO.Path.GetDirectoryName (Application.absoluteURL).Replace ("\\", "/") + "/StreamingAssets";
		else if (Application.isMobilePlatform)
			return Application.persistentDataPath;
		else if (Application.isConsolePlatform)
			return Application.streamingAssetsPath;
		else // For standalone player.
			return "file://" +  Application.streamingAssetsPath;
	}


}
