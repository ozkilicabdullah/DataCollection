using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts.Entites.Base
{
    public enum ReturnReason
    {
        BigSize = 1, // Bedeni büyük geldi
        SmallSize = 2, // Bedeni küçük geldi
        ProductQuality = 3,// Ürün kalitesini beğenmedim
        ColorMisMatch = 4, // Rengini beğenmedim
        RightOfWithdrawal = 5, // Tüketici cayma hakkı
    }
}
