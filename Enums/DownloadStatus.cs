using System.ComponentModel;

namespace CurlFFmpeg.Enums;

public enum DownloadStatus
{
    [Description("En attente")]
    Pending,

    [Description("En cours")]
    Downloading,

    [Description("Terminé")]
    Completed,

    [Description("En pause")]
    Paused,

    [Description("Échec")]
    Failed
}
