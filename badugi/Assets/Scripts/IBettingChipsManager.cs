using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBettingChipsManager : MonoBehaviour {

    public static IBettingChipsManager Instance;

    [SerializeField]
    GameObject[] m_objPlayerPos = null;

    [SerializeField]
    GameObject m_objbettingChips;

    List<GameObject> m_lstChips = new List<GameObject>();

    IEnumerator m_coMoveChip;

    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    StartCoroutine(EventManager.Instance.StartTurtle());
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    PlayGameUI.Instance.SetDeckImage(PlayGameUI._eDeckImageType.EDIT_EVENT);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    AniManager.Instance.PlayMovie(AniManager._eType.CEREMONY_X200, false);
        //}
    }

    public void AddChip(GameObject obj)
    {
        m_lstChips.Add(obj);
    }

    public void SortingLayer(string strLayer)
    {
        for (int i = 0; i < m_lstChips.Count; ++i)
        {
            m_lstChips[i].GetComponent<SpriteRenderer>().sortingLayerName = strLayer;
        }
    }

    public void MoveChip(int iIndex)
    {
        m_coMoveChip = CoMoveChipToWinner(iIndex);
        StartCoroutine(m_coMoveChip);
    }

    public IEnumerator CoMoveChipToWinner(int iIndex)
    {
        SortingLayer("High2");
        Vector3 target = new Vector3();
        target.x = m_objPlayerPos[iIndex].transform.position.x;
        target.y = m_objPlayerPos[iIndex].transform.position.y;
        target.z = 0.0f;
        LeanTween.move(IInstantiateParent.Instance.m_objWinnerChip, target, 0.2f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(IInstantiateParent.Instance.m_objWinnerChip, new Vector3(0.7f, 0.7f, 0.7f), 0.2f).setEase(LeanTweenType.easeInOutSine);

        if( iIndex == 0 )
        {
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.WINNERCEREMONY, false);
        }
        yield return new WaitForSeconds(0.3f);
        LeanTween.value(gameObject, SetSpriteAlpha_WinnerChips, 1f, 0f, 0.3f);
        yield return new WaitForSeconds(0.32f);
        for (int i = 0; i < m_lstChips.Count; ++i)
        {
            m_lstChips[i].SetActive(false);
            DestroyObject(m_lstChips[i]);
        }
        m_lstChips.Clear();
        IInstantiateParent.Instance.m_objWinnerChip.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        IInstantiateParent.Instance.m_objWinnerChip.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        StartCoroutine(PlayGameUI.Instance.player_info_slots[iIndex].CoPlayWinnerEffect());
    }

    void SetSpriteAlpha_WinnerChips(float val)
    {
        for (int i = 0; i < m_lstChips.Count; ++i)
        {
            SpriteRenderer sprite = m_lstChips[i].GetComponent<SpriteRenderer>();
            sprite.color = new Color(1f, 1f, 1f, val);
        }
    }

    public void Clear()
    {
        if (m_coMoveChip != null)
            StopCoroutine(m_coMoveChip);

        for (int i = 0; i < m_lstChips.Count; ++i)
        {
            m_lstChips[i].SetActive(false);
            DestroyObject(m_lstChips[i]);
        }
        m_lstChips.Clear();
    }
}
