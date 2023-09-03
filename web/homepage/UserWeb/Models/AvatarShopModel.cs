using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class AvatarShopModel
    {
        public DBLIB.V_WEB_ShopItem[] webShopItemListAvata;
        public DBLIB.V_WEB_ShopItem[] webShopItemListCard;
        public bool Login;
        public int Cash;
        public long GameMoney;
        public long GameMoney2;
        public long SafeMoney;
        public long Mileage;

        public short View;
    }
}