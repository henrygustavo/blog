namespace Blog.DataAccess.Mappings
{
    using Business.Entity;
    using FluentNHibernate.Mapping;

    public class CategorySettingMap : ClassMap<CategorySetting>
    {
        public CategorySettingMap()
        {
            Table("dbo.[CategorySetting]");

            Id(x => x.Id).Column("IdCategorySetting");

            Map(x => x.Name);
        }
    }
}
