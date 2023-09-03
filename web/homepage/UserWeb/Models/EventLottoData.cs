using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class EventLottoData
    {
        public DBLIB.EventLottoResult LottoResult; // 이벤트 결과
        public DBLIB.EventLottoBase Lotto; // 응모 가능한 이벤트
        public int LottoResultCount; // 이벤트 결과 목록
        public bool UserData; // 유저
        public int UserLottoCount; // 유저가 보유중인 응모권
        public List<DBLIB.EventLottoEnter> UserLottoEnterList; // 유저가 응모한 내역
        
        public int pageNo;
        public int startPage;
        public int totalPages;
    }
}