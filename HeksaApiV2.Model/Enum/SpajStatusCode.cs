using System.ComponentModel.DataAnnotations;

namespace HeksaApiV2.Model.Enum
{
    public enum SpajStatusCode
    {
        DRAFT = 113,
        OPEN = 0,
        PROSES = 1,
        INFORCE = 2,
        AKSEPTASI = 6,
        REJECT = 112,
        PENDING = 114,
        CANCEL = 105
    }
}