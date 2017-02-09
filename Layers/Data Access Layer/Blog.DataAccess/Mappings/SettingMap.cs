namespace Blog.DataAccess.Mappings
{
    using Business.Entity;
    using FluentNHibernate.Mapping;

    public class SettingMap : ClassMap<Setting>
    {
        public SettingMap()
        {
            Table("dbo.[Setting]");

            Id(x => x.Id).Column("IdSetting");

            Map(x => x.Name);

            Map(x => x.ParamValue);

            Map(x => x.IdCategorySetting);
        }
    }
}