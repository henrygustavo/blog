namespace Blog.DataAccess.Mappings
{
    using Business.Entity;
    using FluentNHibernate.Mapping;

    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("dbo.[Roles]");

            Id(x => x.Id).Column("IdRole");

            Map(x => x.Name);
        }
    }
}