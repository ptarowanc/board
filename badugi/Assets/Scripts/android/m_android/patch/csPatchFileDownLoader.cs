using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using UnityEngine.Networking;

public struct st_PatchInfo
{
	public int m_nVer;
	public string m_StrName;
	public string m_StrURL;
	public AssetBundle m_Bundle;

	public void SetInfo(string name, int ver, string url,string ext)
	{
		m_nVer = ver;
		m_StrName = name;
		m_StrURL = url;
	}

	public void SetAssetBundle(AssetBundle bundle)
	{
		m_Bundle = bundle;
	}
}

public class csPatchFileDownLoader : MonoBehaviour {

	public enum enSTATE{ NONE, ING,FINISH,NET_ERROR }

	#region Delegate Define
	public delegate void StateEvent(enSTATE state,float progress,string msg);
	public static StateEvent OnStateChange;
	#endregion

	#region Member Variable
	bool m_bIsDone = true;
	public bool IsDone
	{
		get{ return m_bIsDone; }
	}
	#endregion

	#region MonoBase Function

	void OnDestroy()
	{
		OnStateChange = null;
	}
	#endregion

	#region Custom Function
	public void StartDownLoad(string PatchFileURL)
	{

		if (m_bIsDone)
		{
			m_bIsDone = false;

			StartCoroutine ("CoDownLoad",PatchFileURL);

		}
	}
	IEnumerator CoDownLoad(string PatchFileURL)
	{
		OnStateChange (enSTATE.ING, 0,"파일 리스트 검사 중...");

        csWWWiki.RESET_ARGS();
        csWWWiki.Instance.GET(PatchFileURL);
        while (!csWWWiki.RECV)
        {
            yield return null;
        }

        if (csWWWiki.RECV_CODE == csFlagDef.NET_RECV_CODE.S_OK)
        {
            if (csWWWiki.RECV_VAL.Length == 0)
            {
                m_bIsDone = true;
                OnStateChange(enSTATE.NET_ERROR, 0, "패치파일 정보가 없습니다.\n(update_version.xml)");
            }
            else
            {
                ArrayList PatchFileList = ParserXMLForUpdateVersion(csWWWiki.RECV_VAL.Trim());

                if (PatchFileList.Count > 0)
                {
                    StartCoroutine(CoPatchListDownLoad(PatchFileList));
                }
                else
                {
                    m_bIsDone = true;
                    OnStateChange(enSTATE.FINISH, 1, "최신 파일 입니다.");
                }

            }
        }
        else
        {
            m_bIsDone = true;
            OnStateChange(enSTATE.NET_ERROR, 0, "네트워크상태가 원활하지 않습니다.\n잠시 후 다시 이용해 주세요.\n(Code:update_version.xml)");
        }
        /*
        using (WWW www = WWW.LoadFromCacheOrDownload(PatchFileURL,2))
        {
            yield return www;

            if (www.error != null)
            {
                m_bIsDone = true;
                OnStateChange(enSTATE.NET_ERROR, 0, www.error.ToString());
            }
            else
            {
                AssetBundle bundle = www.assetBundle;

                TextAsset xml = bundle.LoadAsset("update_version", typeof(TextAsset)) as TextAsset;

                if (xml != null)
                {
                    ArrayList PatchFileList = ParserXMLForUpdateVersion(xml.text.Trim());

                    if (PatchFileList.Count > 0)
                    {
                        StartCoroutine(CoPatchListDownLoad(PatchFileList));
                    }
                    else
                    {
                        m_bIsDone = true;
                        OnStateChange(enSTATE.FINISH, 1, "최신 파일 입니다.");
                    }
                }
                www.Dispose();

            }

        }
        */
        /*
        using (WWW www = new WWW (PatchFileURL))
        {
            yield return www;

            if (www.error != null)
            {
                m_bIsDone = true;
                OnStateChange (enSTATE.NET_ERROR,0,www.error.ToString());
            }
            else
            {
                AssetBundle bundle = www.assetBundle;

                TextAsset xml = bundle.LoadAsset ("update_version", typeof(TextAsset)) as TextAsset;

                if (xml != null)
                {
                    ArrayList PatchFileList = ParserXMLForUpdateVersion (xml.text.Trim());

                    if (PatchFileList.Count > 0)
                    {
                        StartCoroutine (CoPatchListDownLoad (PatchFileList));
                    }
                    else
                    {
                        m_bIsDone = true;
                        OnStateChange (enSTATE.FINISH, 1, "최신 파일 입니다.");
                    }
                }
                www.Dispose();

            }

        }
        */
    }
	IEnumerator CoPatchListDownLoad(ArrayList PatchFileList)
	{
        while (!Caching.ready)
            yield return null;

        bool bErr = false;
		for(int i = 0 ; i < PatchFileList.Count ; i++)
		{
			st_PatchInfo info = (st_PatchInfo)PatchFileList [i];

            using (WWW www = WWW.LoadFromCacheOrDownload(info.m_StrURL, info.m_nVer))
            {
                while (!www.isDone)
                {
                    string StrMsg = string.Format("패치 작업 진행 중... ( {0} / {1} )\n현재진행  : {2}%", i, PatchFileList.Count, Mathf.RoundToInt((float)Math.Round(www.progress, 2) * 100));
                    OnStateChange(enSTATE.ING, www.progress, StrMsg);

                    yield return null;

                }
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    bErr = true;
                    OnStateChange(enSTATE.NET_ERROR, 0, www.error.ToString());
                }
                else
                {

                    //SaveAssetBundle(www.bytes, info.m_StrName);

                    System.GC.Collect();
                    if (www.assetBundle != null)
                    {
                        if (info.m_StrName.Equals("img_store.unity3d"))
                        {
                            csAndroidManager.StoreAsset = www.assetBundle;
                        }
                        else if (info.m_StrName.Equals("img_avatar.unity3d"))
                        {
                            csAndroidManager.AvatarAsset = www.assetBundle;
                        }
                        else if (info.m_StrName.Equals("img_card.unity3d"))
                        {
                            csAndroidManager.CardAsset = www.assetBundle;
                        }
                        //www.assetBundle.Unload(true);
                    }
                    www.Dispose();
                }
            }
            
            /*
			using (WWW www = new WWW(info.m_StrURL))
			{
				while(!www.isDone)
				{
					string StrMsg = string.Format ("패치 작업 진행 중... ( {0} / {1} )\n현재진행  : {2}%", i, PatchFileList.Count,Mathf.RoundToInt ((float)Math.Round (www.progress, 2) * 100));
					OnStateChange (enSTATE.ING, www.progress, StrMsg);

					yield return null;

				}
				yield return www;

				if (www.error != null)
				{
					bErr = true;
					OnStateChange (enSTATE.NET_ERROR, 0 , www.error.ToString());
				}
				else
				{

					SaveAssetBundle( www.bytes, info.m_StrName);	


					System.GC.Collect();
					if(www.assetBundle != null)
					{
						www.assetBundle.Unload(true);
					}
					www.Dispose();
				}
			}
            */
            if (bErr)
			{
				break;
			}
			yield return null;
		}

		m_bIsDone = true;
		if (!bErr)
		{
			//SaveVersionInfo (PatchFileList);
			OnStateChange (enSTATE.FINISH, 1, "패치 작업 완료.");
		}
	}
    ArrayList ParserXMLForUpdateVersion(string Packet)
    {
        //Hashtable HtLocalCodeVers = LoadVersionInfo();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Packet);

        ArrayList ArrUpdateGroup = new ArrayList();
        XmlNodeList nodes = xmlDoc.SelectNodes("update/code");
        foreach (XmlNode node in nodes)
        {
            ArrUpdateGroup.Add(node);
        }

        ArrayList UpdatePatchList = new ArrayList();
        ArrayList ArrUpdateCode = new ArrayList();
        foreach (XmlNode node in ArrUpdateGroup)
        {
            //nodes = node.SelectNodes ("code");
            //foreach (XmlNode node_code in nodes)
            {
                XmlNode code_ver = node.SelectSingleNode("ver");
                string name = node.Attributes["name"].InnerText.ToLower();
                int version = int.Parse(code_ver.InnerText);
                string url = node.SelectSingleNode("url").InnerText.ToLower();

                //object obj_ver = HtLocalCodeVers[name];

                //bool bUpdate = false;
                //if (obj_ver == null)
                //{
                //    bUpdate = true;
                //}
                //else
                //{
                //    if (int.Parse((string)obj_ver) < version)
                //    {
                //        bUpdate = true;
                //    }
                //}

                st_PatchInfo info = new st_PatchInfo();
                info.m_nVer = version;
                info.m_StrName = name;
                info.m_StrURL = url;

                //if (bUpdate)
                {
                    if (url.Length > 0)
                    {
                        UpdatePatchList.Add(info);
                    }
                }
            }
        }

        return UpdatePatchList;
    }
    /*
	ArrayList ParserXMLForUpdateVersion(string Packet)
	{
		Hashtable HtLocalCodeVers = LoadVersionInfo ();

		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (Packet);

		ArrayList ArrUpdateGroup = new ArrayList ();
		XmlNodeList nodes = xmlDoc.SelectNodes ("update/code");
		foreach (XmlNode node in nodes)
		{
			ArrUpdateGroup.Add (node);
		}

		ArrayList UpdatePatchList = new ArrayList ();
		ArrayList ArrUpdateCode = new ArrayList ();
		foreach (XmlNode node in ArrUpdateGroup)
		{
			//nodes = node.SelectNodes ("code");
			//foreach (XmlNode node_code in nodes)
			{
				XmlNode code_ver = node.SelectSingleNode ("ver");
				string name = node.Attributes ["name"].InnerText.ToLower ();
				int version = int.Parse (code_ver.InnerText);
				string url = node.SelectSingleNode ("url").InnerText.ToLower ();

				object obj_ver = HtLocalCodeVers [name];

				bool bUpdate = false;
				if (obj_ver == null)
				{
					bUpdate = true;
				}
				else
				{
					if (int.Parse ((string)obj_ver) < version)
					{
						bUpdate = true;
					}
				}

				st_PatchInfo info = new st_PatchInfo ();
				info.m_nVer = version;
				info.m_StrName = name;
				info.m_StrURL = url;

				if (bUpdate)
				{
					if (url.Length > 0)
					{
						UpdatePatchList.Add (info);
					}
				}
			}
		}

		return UpdatePatchList;
	}
    */
    void SaveAssetBundle( byte[] byteData, string fileName)
	{
		string savePath = csBundleLoader.GetRelativePath() +"/" + fileName;
		FileStream fs = new FileStream( savePath , FileMode.Create);
		fs.Seek(0, SeekOrigin.Begin);
		fs.Write(byteData, 0, byteData.Length);
		fs.Close();
	}

	Hashtable LoadVersionInfo()
	{
		Hashtable HtLocalCodeVers = new Hashtable ();
		string _FileLocation = csBundleLoader.GetRelativePath();
		string _FileName = "version_info.txt";
		FileInfo t = new FileInfo(_FileLocation + "/" + _FileName);
		StreamReader _reader = null;
		if (t.Exists)
		{
			_reader = t.OpenText();
		}

		if (_reader != null)
		{
			string info = "";
			do
			{
				info = _reader.ReadLine();
				if (info != null)
				{
					if (info.Contains("_code_version:"))
					{
						int nPos = info.IndexOf("_code_version:");
						string str_code = info.Substring(0, nPos);
						nPos = info.IndexOf(':');
						string ver = info.Substring(nPos+1);

						HtLocalCodeVers.Add(str_code, ver);
					}
				}
			}  while (info != null);

			_reader.Close();
			_reader = null;
		}

		return HtLocalCodeVers;
	}
	void SaveVersionInfo(ArrayList list)
	{
		string _FileLocation = csBundleLoader.GetRelativePath();
		string _FileName = "version_info.txt";
		FileInfo t = new FileInfo(_FileLocation + "/" + _FileName);
		StreamWriter _writer = null;
		if (!t.Exists)
		{
			_writer = t.CreateText();
		}
		else
		{
			t.Delete();
			_writer = t.CreateText();
		}

		for(int i = 0 ; i < list.Count ; i++)
		{
			st_PatchInfo info = (st_PatchInfo)list[i];
			_writer.WriteLine(info.m_StrName + "_code_version:" + info.m_nVer);
		}
		_writer.Close();
		_writer = null;
	}
	#endregion

}
