using SwEpApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants.Base.IYS.RequestParams
{
    public class IYSUpdateRequestParams : ModelListBase
    {
        public string consentDate { get; set; } // İznin kullanıcıdan alındığı tarihtir
        public string creationDate { get; set; } // İznin IYS’ye kaydedilme tarihi.
        public string recipient { get; set; } // Kullanıcının sistemde kayıtlı telefon numarası veya e-posta bilgisidir / Telefon numarası ->E164 uluslararası([+][country code][area code][local phone number]
        public string recipientType { get; set; } // “BIREYSEL” ya da “TACIR”
        public string status { get; set; } // “ONAY” ya da “RET”
        public string type { get; set; } // “EPOSTA”, “ARAMA” ya da “MESAJ”
    }
}
