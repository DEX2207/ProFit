using System.ComponentModel;

namespace ProFit.Domain.Enum;

public enum Status
{
    [Description("Получен")]
    Received=0,
    [Description("Действует")]
    Valid=1,
    [Description("Закончился")]
    Ended=2,
}