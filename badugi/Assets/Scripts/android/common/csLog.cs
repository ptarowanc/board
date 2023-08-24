
using UnityEngine;
using System.Collections;

public class csLog{

    /// <summary>
    /// Debug Type's
    /// </summary>
    public enum TYPE { DEBUG,ERROR}

    /// <summary>
    /// Execute Debug.Log
    /// </summary>
	/// <param name="ArgClass">Class Name</param>
	/// <param name="ArgFunc">Function Name</param>
    /// <param name="ArgText">Print Text</param>
    /// <param name="ArgType">Debug Type</param>
	public static void Show(string ArgClass,string ArgFunc, string ArgText,TYPE ArgType = TYPE.DEBUG)
    {
		string msg = string.Format ("[{0}] [{1}] {2}", ArgClass, ArgFunc, ArgText);
#if DEBUG
        if(ArgType == TYPE.ERROR)
        {
			Debug.LogError(msg);
            return;
        }
		Debug.Log(msg);
#endif
    }
	/// <summary>
	/// Execute Debug.Log
	/// </summary>
	/// <param name="ArgText"></param>
	/// <param name="ArgType"></param>
	public static void Show(string ArgText,TYPE ArgType = TYPE.DEBUG)
	{
		#if DEBUG
		if(ArgType == TYPE.ERROR)
		{
			Debug.LogError(ArgText);
			return;
		}
		Debug.Log(ArgText);
		#endif
	}
}
