namespace TheResistanceOnline.Data.Entities;

public class RedditUser: Entity<RedditId>
{
    public User User { get; set; }

    public UserId UserId { get; set; }

    public RedditUser()
    {
    }

    private RedditUser(RedditId redditId): base(redditId)
    {
    }

    public static RedditUser Create(RedditId redditId)
    {
        var redditUser = new RedditUser(redditId);
        var user = User.Create();
        redditUser.User = user;
        user.RedditUser = redditUser;

        return redditUser;
    }
}
