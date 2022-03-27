namespace Moonpie.Protocol.Protocol;

public class Team
{
    public enum TeamCollisionRules
    {
        Always,
        PushEnemies,
        PushMates,
        Never
    }

    public enum NameTagVisibilities
    {
        Always,
        HideForEnemies,
        HideForMates,
        Never
    }
    
    public string Name { get; set; } = "";
    public ChatComponent DisplayName = ChatComponent.Empty;
    public ChatComponent Prefix = ChatComponent.Empty;
    public ChatComponent Suffix = ChatComponent.Empty;
    public bool FriendlyFire = true;
    public bool CanSeeInvisibleTeamMembers = true;
    public TeamCollisionRules CollisionRule { get; set; } = TeamCollisionRules.Always;
    public NameTagVisibilities NameTagVisibility { get; set; } = NameTagVisibilities.Always;
    public string? FormattingCode { get; set; }
    public string[] Members { get; set; } = new string[0];
}