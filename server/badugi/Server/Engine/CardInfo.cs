using System;
using System.Collections.Generic;

namespace Server.Engine
{
    public class Stringtable
    {
        public string dSTR_RES_NOMADE_NF = "노페어";
        public string dSTR_RES_TWOBASE_NF = "투베이스";
        public string dSTR_RES_BASE_NF = "베이스";
        public string dSTR_RES_MADE_NF = "메이드";
        public string dSTR_RES_THIRD_NF = "써드";
        public string dSTR_RES_SECOND_NF = "세컨드";
        public string dSTR_RES_GOLF_NF = "골프";
    }
    public class CardInfo
    {
        public Stringtable cST = new Stringtable();
        public byte dMAX_CARD_SHAPE = 4;
        public byte dMAX_CARD_CNT = 13;
        public byte dMAX_PLAYER_CARD = 4;
        public byte dCARD_STATE_DISABLE;
        public byte[,] m_btCard;//카드정보
        public byte[] m_btNumberCnt;//카드 숫자
        public byte[] m_btShapeCnt;// 카드 무늬
        public int[,] m_nFindCard;//찾은 카드
        public bool[] m_nFindCardShape;//찾은 카드 무늬
        public byte[] m_nFindCardNumber;//찾은 카드 숫자
        public byte[] m_btCardNameCnt;
        public int m_nResult;
        public int m_nTopNumber;
        public int m_nTopNumber2;
        public CardInfo.sCARD_INFO[] m_sCard;

        public CardInfo()
        {
            this.m_btCard = new byte[(int)this.dMAX_CARD_SHAPE, (int)this.dMAX_CARD_CNT];
            this.m_btNumberCnt = new byte[(int)this.dMAX_CARD_CNT];
            this.m_btShapeCnt = new byte[(int)this.dMAX_CARD_CNT];
            this.m_nFindCard = new int[(int)this.dMAX_CARD_SHAPE, (int)this.dMAX_CARD_CNT];
            this.m_nFindCardShape = new bool[(int)this.dMAX_PLAYER_CARD];
            this.m_nFindCardNumber = new byte[(int)this.dMAX_PLAYER_CARD];
            this.m_btCardNameCnt = new byte[(int)this.dMAX_CARD_CNT];

            this.m_sCard = new CardInfo.sCARD_INFO[(int)this.dMAX_PLAYER_CARD];
        }

        public bool FindCard(int nCard, int nCnt)
        {
            int num = 0;
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
            {
                if (this.m_nFindCard[i, nCard] > -1)
                {
                    ++num;
                    this.m_nFindCardShape[this.m_nFindCard[i, nCard]] = true;
                    if (num >= nCnt)
                        return true;
                }
            }
            return false;
        }

        public bool FindShape(int nShape, int nCnt)
        {
            int num = 0;
            for (int i = 0; i < (int)this.dMAX_CARD_CNT; ++i)
            {
                if (this.m_nFindCard[nShape, i] > -1)
                {
                    ++num;
                    this.m_nFindCardShape[this.m_nFindCard[nShape, i]] = true;
                    if (num >= nCnt)
                        return true;
                }
            }
            return false;
        }

        public bool FindHighMark(int nShape, int nNumCnt)
        {
            if (nShape < 0)
                return false;
            for (int i = (int)this.dMAX_CARD_CNT - 1; i > -1; --i)
            {
                if (this.m_nFindCard[nShape, i] > -1 && (int)this.m_btNumberCnt[i] == nNumCnt)
                {
                    this.m_nFindCardShape[this.m_nFindCard[nShape, i]] = true;
                    return true;
                }
            }
            return false;
        }

        public bool FindOtherMark(int nCard)
        {
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
            {
                if (this.m_nFindCard[i, nCard] > -1 && this.m_nFindCardShape[this.m_nFindCard[i, nCard]])
                    return true;
            }
            return false;
        }

        public bool FindLowMark(int nShape, int nNumCnt)
        {
            if (nShape < 0)
                return false;
            for (int i = 0; i < (int)this.dMAX_CARD_CNT; ++i)
            {
                if (this.m_nFindCard[nShape, i] > -1 && (int)this.m_btNumberCnt[i] == nNumCnt)
                {
                    this.m_nFindCardShape[this.m_nFindCard[nShape, i]] = true;
                    return true;
                }
            }
            return false;
        }

        public string GetCardName()
        {
            string str1 = "";
            string[] strArray = new string[13]
            { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            byte[] numArray = new byte[13];
            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
            {
                if (this.m_nFindCardShape[i])
                    numArray[this.m_sCard[i].m_nCardNum] = (byte)1;
            }
            for (int i = 0; i < (int)this.dMAX_CARD_CNT; ++i)
            {
                if ((int)numArray[i] != 0)
                {
                    string str2 = strArray[i];
                    str1 = str1 + str2 + " ";
                }
            }
            return str1;
        }

        public int GetResult()
        {
            switch (this.m_nResult)
            {
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 4:
                    return 4;
                case 5:
                    return 5;
                case 6:
                    return 6;
                case 7:
                    return 7;
                default:
                    return 1;
            }
        }

        public string GetVoiceFileName()
        {
            string[] strArray = new string[13]
            { "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" };
            string str;
            switch (this.m_nResult)
            {
                case 2:
                    str = "TwoBase";
                    break;
                case 3:
                    str = "Base";
                    break;
                case 4:
                    str = "Made";
                    break;
                case 5:
                    return "Made_Third";
                case 6:
                    return "Made_Second";
                case 7:
                    return "Made_Golf";
                default:
                    return "NoBase";
            }
            int topNumber = this.GetTopNumber();
            if (topNumber != 0)
                str = str + "_" + strArray[topNumber];
            return str;
        }
        public int GetTotalScore()
        {
            int totalscore = 0;

            int topNumber = this.GetTopNumber();
            if (topNumber != 0)
                totalscore = (GetResult() * 1000) + ((13 - topNumber) * 10);
            return totalscore;
        }
        public int GetTopNumber()
        {
            int num = -1;
            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
            {
                if (this.m_nFindCardShape[i])
                {
                    if (num == -1)
                        num = this.m_sCard[i].m_nCardNum;
                    else if (num < this.m_sCard[i].m_nCardNum)
                        num = this.m_sCard[i].m_nCardNum;
                }
            }
            if (num == -1)
            {
                for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
                {
                    if (num < this.m_sCard[i].m_nCardNum)
                        num = this.m_sCard[i].m_nCardNum;
                }
            }
            return num;
        }
        public int GetTopNumber2()
        {
            int num = -1;
            int num2 = -1;
            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
            {
                if (this.m_nFindCardShape[i])
                {
                    if (num == -1)
                        num = this.m_sCard[i].m_nCardNum;
                    else if (num < this.m_sCard[i].m_nCardNum)
                        num = this.m_sCard[i].m_nCardNum;

                    if (num2 == -1)
                        num2 = this.m_sCard[i].m_nCardNum;
                    else if (this.m_sCard[i].m_nCardNum < num2 && num2 > num)
                        num2 = this.m_sCard[i].m_nCardNum;
                }
            }
            if (num2 == num)
            {
                num2 = -1;
            }
            return num2;
        }
        public int GetTotalHandScore()
        {
            int totalscore = 0;
            int index = 0;
            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
            {
                if (this.m_nFindCardShape[i])
                {
                    this.m_nFindCardNumber[index++] = (byte)(this.m_sCard[i].m_nCardNum+1);
                }
                else
                {
                    this.m_nFindCardNumber[index++] = 14;
                }
            }

            Array.Sort(m_nFindCardNumber);

            for( int i = 0; i < m_nFindCardNumber.Length-1; ++i)
            {
                if(m_nFindCardNumber[i] == 14)
                {
                    m_nFindCardNumber[i] = m_nFindCardNumber[i + 1];
                    m_nFindCardNumber[i + 1] = 14;
                }
            }
            
            totalscore = (GetResult() * 100000000) + ((14 - m_nFindCardNumber[3]) * 1000000) + ((14 - m_nFindCardNumber[2]) * 10000) + ((14 - m_nFindCardNumber[1]) * 100) + ((14 - m_nFindCardNumber[0]));
            return totalscore;
        }

        public void ClearFindCardShape()
        {
            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
                this.m_nFindCardShape[i] = false;
        }

        public void Clear()
        {
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
            {
                for (int j = 0; j < (int)this.dMAX_CARD_CNT; ++j)
                    this.m_btCard[i, j] = (byte)0;
            }
            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
            {
                this.m_sCard[i].m_btIsState = this.dCARD_STATE_DISABLE;
                this.m_sCard[i].m_nShape = -1;
                this.m_sCard[i].m_nCardNum = -1;
            }
            for (int i = 0; i < (int)this.dMAX_CARD_CNT; ++i)
                this.m_btNumberCnt[i] = (byte)0;
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
                this.m_btShapeCnt[i] = (byte)0;
            this.m_nResult = 1;
            this.m_nTopNumber = 14;
            this.m_nTopNumber2 = 13;
        }

        public void SetCard(CardInfo.sCARD_INFO[] pCard)
        {
            if (pCard.Length == 0) return;

            //초기화
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
            {
                for (int j = 0; j < (int)this.dMAX_CARD_CNT; ++j)
                    this.m_nFindCard[i, j] = -1;
            }

            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
            {
                if (pCard[i].m_nShape != -1 && pCard[i].m_nCardNum != -1)
                {
                    this.m_btCard[pCard[i].m_nShape, pCard[i].m_nCardNum] = (byte)1;
                    ++this.m_btNumberCnt[pCard[i].m_nCardNum];
                    ++this.m_btShapeCnt[pCard[i].m_nShape];
                    this.m_nFindCard[pCard[i].m_nShape, pCard[i].m_nCardNum] = i;
                    this.m_sCard[i].m_btIsState = pCard[i].m_btIsState;
                    this.m_sCard[i].m_nCardNum = pCard[i].m_nCardNum;
                    this.m_sCard[i].m_nShape = pCard[i].m_nShape;
                }
            }
        }

        public void MakeResult()
        {
            if (this.IsGolf())
                this.m_nResult = 7;
            else if (this.IsSecond())
                this.m_nResult = 6;
            else if (this.IsThird())
                this.m_nResult = 5;
            else if (this.IsMade())
                this.m_nResult = 4;
            else if (this.IsBase())
                this.m_nResult = 3;
            else if (this.IsTwoBase())
                this.m_nResult = 2;
            else if (this.IsNoBase())
                this.m_nResult = 1;
            else
                this.m_nResult = 0;

            this.m_nTopNumber = GetTopNumber();
            this.m_nTopNumber2 = GetTopNumber2();
        }
        public void AutoSelect(bool[] btSeletedCard)
        {
            btSeletedCard[0] = !this.m_nFindCardShape[0];
            btSeletedCard[1] = !this.m_nFindCardShape[1];
            btSeletedCard[2] = !this.m_nFindCardShape[2];
            btSeletedCard[3] = !this.m_nFindCardShape[3];
        }

        public bool IsGolf()
        {
            this.ClearFindCardShape();
            if ((int)this.m_btNumberCnt[0] == 0 || (int)this.m_btNumberCnt[1] == 0 || ((int)this.m_btNumberCnt[2] == 0 || (int)this.m_btNumberCnt[3] == 0))
                return false;
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
            {
                if ((int)this.m_btShapeCnt[i] > 1)
                    return false;
            }
            this.FindCard(0, 1);
            this.FindCard(1, 1);
            this.FindCard(2, 1);
            this.FindCard(3, 1);
            return true;
        }

        public bool IsSecond()
        {
            this.ClearFindCardShape();
            if ((int)this.m_btNumberCnt[0] == 0 || (int)this.m_btNumberCnt[1] == 0 || ((int)this.m_btNumberCnt[2] == 0 || (int)this.m_btNumberCnt[4] == 0))
                return false;
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
            {
                if ((int)this.m_btShapeCnt[i] > 1)
                    return false;
            }
            this.FindCard(0, 1);
            this.FindCard(1, 1);
            this.FindCard(2, 1);
            this.FindCard(4, 1);
            return true;
        }

        public bool IsThird()
        {
            this.ClearFindCardShape();
            if ((int)this.m_btNumberCnt[0] == 0 || (int)this.m_btNumberCnt[1] == 0 || ((int)this.m_btNumberCnt[3] == 0 || (int)this.m_btNumberCnt[4] == 0))
                return false;
            for (int i = 0; i < (int)this.dMAX_CARD_SHAPE; ++i)
            {
                if ((int)this.m_btShapeCnt[i] > 1)
                    return false;
            }
            this.FindCard(0, 1);
            this.FindCard(1, 1);
            this.FindCard(3, 1);
            this.FindCard(4, 1);
            return true;
        }

        public bool IsMade()
        {
            this.ClearFindCardShape();
            for (int i = 0; i < (int)this.dMAX_PLAYER_CARD; ++i)
            {
                if (this.m_sCard[i].m_nShape == -1 || this.m_sCard[i].m_nCardNum == -1)
                    return false;
            }
            for (int nShape = 0; nShape < (int)this.dMAX_CARD_SHAPE; ++nShape)
            {
                if ((int)this.m_btShapeCnt[nShape] > 1)
                    return false;
                if ((int)this.m_btShapeCnt[nShape] == 1)
                    this.FindShape(nShape, 1);
            }
            for (int nCard = 0; nCard < (int)this.dMAX_CARD_CNT; ++nCard)
            {
                if ((int)this.m_btNumberCnt[nCard] > 1)
                    return false;
                if ((int)this.m_btNumberCnt[nCard] == 1)
                    this.FindCard(nCard, 1);
            }
            return true;
        }

        public bool IsBase()
        {
            this.ClearFindCardShape();
            int num1 = 0;
            int nCard1 = 0;
            for (int i = 0; i < (int)this.dMAX_CARD_CNT; ++i)
            {
                if ((int)this.m_btNumberCnt[i] == 2)
                {
                    ++num1;
                    nCard1 = i;
                }
                else if ((int)this.m_btNumberCnt[i] > 2)
                    return false;
                if (num1 == 2)
                    return false;
            }
            int num2 = 0;
            int nMark1 = -1;
            for (int nMark2 = 0; nMark2 < (int)this.dMAX_CARD_SHAPE; ++nMark2)
            {
                if ((int)this.m_btShapeCnt[nMark2] == 1)
                {
                    this.FindShape(nMark2, 1);
                    ++num2;
                }
                else if ((int)this.m_btShapeCnt[nMark2] == 2)
                {
                    nMark1 = nMark2;
                    ++num2;
                }
            }
            if (num2 < 3)
                return false;
            if (num2 == 3)
                return (num1 != 1 || this.ExistCardAndMark(nCard1, nMark1)) && this.FindLowMark(nMark1, 1);
            if (num1 == 0)
                return false;
            this.ClearFindCardShape();
            for (int nCard2 = 0; nCard2 < (int)this.dMAX_CARD_CNT; ++nCard2)
            {
                if ((int)this.m_btNumberCnt[nCard2] == 1)
                    this.FindCard(nCard2, 1);
                else if ((int)this.m_btNumberCnt[nCard2] == 2)
                    this.FindCard(nCard2, 1);
            }
            return true;
        }

        public bool IsTwoBase()
        {
            this.ClearFindCardShape();
            int num1 = 0;
            int[] numArray1 = new int[2] { -1, -1 };
            int num2 = 0;
            for (int index = 0; index < (int)this.dMAX_CARD_CNT; ++index)
            {
                if ((int)this.m_btNumberCnt[index] == 2)
                    numArray1[num1++] = index;
                else if ((int)this.m_btNumberCnt[index] == 3)
                    ++num2;
                else if ((int)this.m_btNumberCnt[index] == 4)
                    return false;
            }
            int num3 = 0;
            int nMark1 = -1;
            int[] numArray2 = new int[2] { -1, -1 };
            int num4 = 0;
            for (int nMark2 = 0; nMark2 < (int)this.dMAX_CARD_SHAPE; ++nMark2)
            {
                if ((int)this.m_btShapeCnt[nMark2] == 1)
                {
                    this.FindShape(nMark2, 1);
                    ++num3;
                }
                else if ((int)this.m_btShapeCnt[nMark2] == 2)
                {
                    numArray2[num4++] = nMark2;
                    ++num3;
                }
                else if ((int)this.m_btShapeCnt[nMark2] == 3)
                {
                    nMark1 = nMark2;
                    ++num3;
                }
            }
            if (num3 < 2)
                return false;
            if (num3 == 2)
            {
                if (nMark1 > -1)
                {
                    if (!this.FindLowMark(nMark1, 1))
                        return false;
                }
                else
                {
                    if (num4 != 2)
                        return false;
                    if (num1 == 0)
                    {
                        if (!this.FindLowMark(numArray2[0], 1) || !this.FindLowMark(numArray2[1], 1))
                            return false;
                    }
                    else if (num1 == 1)
                    {
                        int num5 = 0;
                        for (int nCard = 0; nCard < (int)this.dMAX_CARD_CNT; ++nCard)
                        {
                            if ((int)this.m_btNumberCnt[nCard] == 1)
                            {
                                if (num5 == 0)
                                {
                                    this.FindCard(nCard, 1);
                                    ++num5;
                                }
                                else if (num5 == 1)
                                {
                                    if (nCard < numArray1[0])
                                    {
                                        this.FindCard(nCard, 1);
                                        break;
                                    }
                                    for (int nMark2 = 0; nMark2 < (int)this.dMAX_CARD_SHAPE; ++nMark2)
                                    {
                                        if (this.m_nFindCard[nMark2, nCard] > -1)
                                        {
                                            this.FindLowMark(nMark2, 2);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else if (num1 == 2 && (!this.FindLowMark(numArray2[0], 2) || !this.FindHighMark(numArray2[1], 2)))
                        return false;
                }
                return true;
            }
            if (num3 == 3)
            {
                this.ClearFindCardShape();
                if (num2 > 0)
                {
                    for (int nMark2 = 0; nMark2 < (int)this.dMAX_CARD_SHAPE; ++nMark2)
                    {
                        if ((int)this.m_btShapeCnt[nMark2] == 1)
                        {
                            this.FindShape(nMark2, 1);
                            break;
                        }
                    }
                    if (!this.FindLowMark(numArray2[0], 1))
                        return false;
                }
                else if (num1 == 1)
                {
                    if (!this.FindCard(numArray1[0], 1) || !this.FindLowMark(numArray2[0], 1))
                        return false;
                }
                else
                {
                    if (num1 != 2)
                        return false;
                    for (int nMark2 = 0; nMark2 < (int)this.dMAX_CARD_SHAPE; ++nMark2)
                    {
                        if ((int)this.m_btShapeCnt[nMark2] == 1)
                            this.FindShape(nMark2, 1);
                    }
                }
                return true;
            }
            this.ClearFindCardShape();
            if (num1 == 1)
                return false;
            if (num1 == 2)
            {
                for (int nCard = 0; nCard < (int)this.dMAX_CARD_CNT; ++nCard)
                {
                    if ((int)this.m_btNumberCnt[nCard] == 2)
                        this.FindCard(nCard, 1);
                }
            }
            else if (num1 == 0)
            {
                for (int nCard = 0; nCard < (int)this.dMAX_CARD_CNT; ++nCard)
                {
                    if ((int)this.m_btNumberCnt[nCard] == 1)
                        this.FindCard(nCard, 1);
                    else if ((int)this.m_btNumberCnt[nCard] == 3)
                        this.FindCard(nCard, 1);
                }
            }
            return true;
        }

        public bool IsNoBase()
        {
            this.ClearFindCardShape();
            for (int index = 0; index < (int)this.dMAX_CARD_CNT; ++index)
            {
                if ((int)this.m_btNumberCnt[index] == 4)
                    return true;
            }
            for (int index = 0; index < (int)this.dMAX_CARD_SHAPE; ++index)
            {
                if ((int)this.m_btShapeCnt[index] == 4)
                    return true;
            }
            return false;
        }
        //현재 카드 정보
        public string GetResName()
        {
            switch (this.m_nResult)
            {
                case 2:
                    return this.cST.dSTR_RES_TWOBASE_NF;
                case 3:
                    return this.cST.dSTR_RES_BASE_NF;
                case 4:
                    return this.cST.dSTR_RES_MADE_NF;
                case 5:
                    return this.cST.dSTR_RES_THIRD_NF;
                case 6:
                    return this.cST.dSTR_RES_SECOND_NF;
                case 7:
                    return this.cST.dSTR_RES_GOLF_NF;
                default:
                    return this.cST.dSTR_RES_NOMADE_NF;
            }
        }

        public void CalcTopValue(int[] nArray)
        {
            byte[] numArray = new byte[13];
            nArray[0] = 0;
            nArray[1] = 0;
            nArray[2] = 0;
            nArray[3] = 0;
            for (int index = 0; index < (int)this.dMAX_PLAYER_CARD; ++index)
            {
                if (this.m_nFindCardShape[index])
                    numArray[this.m_sCard[index].m_nCardNum] = (byte)1;
            }
            int num = 12;
            int index1 = 3;
            for (int index2 = (int)this.dMAX_CARD_CNT - 1; index2 >= 0; --index2)
            {
                if ((int)numArray[index2] != 0)
                {
                    nArray[index1] = num - index2;
                    num = index2;
                    --index1;
                    if (index1 < 0)
                        break;
                }
            }
            CardInfo.sCARD_INFO[] array = new CardInfo.sCARD_INFO[4];
            Array.Sort<CardInfo.sCARD_INFO>(array, (x, y) => y.m_nShape.CompareTo(x.m_nShape));
            Array.Sort<CardInfo.sCARD_INFO>(array, (x, y) => x.m_nCardNum.CompareTo(y.m_nCardNum));
        }

        public bool ExistCardAndMark(int nCard, int nMark)
        {
            return nCard != -1 && nMark != -1 && this.m_nFindCard[nMark, nCard] != -1;
        }

        public enum Family
        {
            dRES_NONE,
            dRES_NOMADE,
            dRES_TWOBASE,
            dRES_BASE,
            dRES_MADE,
            dRES_THIRD,
            dRES_SECOND,
            dRES_GOLF,
        }

        public struct sCARD_INFO
        {
            public int m_nShape;        // 스다하크
            public int m_nCardNum;
            public byte m_btIsState;

            public void Init(int m_nMark, int m_nCardNum, byte m_btIsState)
            {
                this.m_nShape = m_nMark;
                this.m_nCardNum = m_nCardNum;
                this.m_btIsState = m_btIsState;
            }
        }

    }
}
