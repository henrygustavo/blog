namespace Blog.DataAccess.Mappings
{
    using Business.Entity;
    using FluentNHibernate.Mapping;

    public class UserViewMap : ClassMap<UserView>
    {
        public UserViewMap()
        {
            Table("dbo.[UsersView]");

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

            Map(x => x.LockoutEnabled);

            Map(x => x.Disabled);

            Map(x => x.AccessFailedCount);

            Map(x => x.CreationDate);

            Map(x => x.LastActivityDate);

            Map(x => x.RoleName);

          
        }
    }
}