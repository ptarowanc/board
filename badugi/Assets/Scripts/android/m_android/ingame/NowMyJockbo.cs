using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NowMyJockbo : MonoBehaviour {

    [SerializeField] Image _golf;
    [SerializeField] Image _golfOff;
    [SerializeField] Image _second;
    [SerializeField] Image _secondOff;
    [SerializeField] Image _third;
    [SerializeField] Image _thirdOff;
    [SerializeField] Image _made;
    [SerializeField] Image _madeOff;
    [SerializeField] Image _base;
    [SerializeField] Image _baseOff;
    [SerializeField] Image _twobase;
    [SerializeField] Image _twobaseOff;

    // Use this for initialization
    void Start () {

        TurnOff();

    }

    public void TurnOff()
    {
        _golf.enabled = false;
        _golfOff.enabled = true;
        _second.enabled = false;
        _secondOff.enabled = true;
        _third.enabled = false;
        _thirdOff.enabled = true;
        _made.enabled = false;
        _madeOff.enabled = true;
        _base.enabled = false;
        _baseOff.enabled = true;
        _twobase.enabled = false;
        _twobaseOff.enabled = true;
    }
    public void UpdateCardInfo(byte _grade)
    {
        Debug.Log("UpdateCardInfo : " + _grade);
        TurnOff();

        if (_grade == 7)
        {
            // 골프
            _golf.enabled = true;
            _golfOff.enabled = false;
        }
        else if (_grade == 6)
        {
            // 세컨
            _second.enabled = true;
            _secondOff.enabled = false;
        }
        else if (_grade == 5)
        {
            // 써드
            _third.enabled = true;
            _thirdOff.enabled = false;
        }
        else if (_grade == 4)
        {
            // 메이드
            _made.enabled = true;
            _madeOff.enabled = false;
        }
        else if (_grade == 3)
        {
            // 베이스
            _base.enabled = true;
            _baseOff.enabled = false;
        }
        else if (_grade == 2)
        {
            // 투베이스
            _twobase.enabled = true;
            _twobaseOff.enabled = false;
        }
    }
}
