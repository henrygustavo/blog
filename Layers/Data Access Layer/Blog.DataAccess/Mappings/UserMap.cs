namespace Blog.DataAccess.Mappings
{
    using Business.Entity;
    using FluentNHibernate.Mapping;

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("dbo.[Users]");

            Id(x => x.Id).Column("IdUser");

            Map(x => x.UserName);

            Map(x => x.Email);

            Map(x => x.EmailConfirmed);

            Map(x => x.PasswordHash);

            Map(x => x.SecurityStamp);

            Map(x => x.PhoneNumber);

            Map(x => x.PhoneNumberConfirmed);

            Map(x => x.TwoFactorEnabled);

            Map(x => x.LockoutEndDateUtc);

            Map(x => x.Disabled);

            Map(x => x.LockoutEnabled);

            Map(x => x.AccessFailedCount);

            Map(x => x.CreationDate);

            Map(x => x.LastActivityDate);
        }
    }
}