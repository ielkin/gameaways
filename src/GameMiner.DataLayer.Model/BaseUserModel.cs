using System;

namespace GameMiner.DataLayer.Model
{
    public abstract class BaseUserModel : BaseModel
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
