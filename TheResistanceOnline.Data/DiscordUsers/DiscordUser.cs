using JetBrains.Annotations;

namespace TheResistanceOnline.Data.DiscordUsers
{
    public class DiscordUser
    {
        public int Id { get; set; }
        
        [CanBeNull]
        public string UserName { get; set; }
        
        
    }
}