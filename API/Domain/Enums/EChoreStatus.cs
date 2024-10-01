using System.ComponentModel;

namespace Domain.Enums
{
    public enum EChoreStatus
    {
        [Description("To Do")]
        ToDo,
        [Description("Doing")]
        Doing,
        [Description("Done")]
        Done
    }
}
