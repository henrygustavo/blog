namespace Blog.DataAccess.Implementations
{
    using Business.Entity;
	using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Linq;

    public class CommonRepository : RepositoryBase<Common>, ICommonRepository
    {
        public DateTime GetCurrentDateTime()
        {
            var result =  Session.CreateSQLQuery("Select getDate()");

            return DateTime.Parse(result.UniqueResult().ToString());
        }

		public List<Setting> GetByIdCategorySetting(int idCategorySetting)
        {
            return Session.Query<Setting>().Where(x => x.IdCategorySetting == idCategorySetting).ToList();
        }

		public Setting GetSetting(int idSetting)
        {
            return Session.Query<Setting>().FirstOrDefault(x => x.Id == idSetting);
        }

		public void InsertCategorySetting(CategorySetting categorySetting)
        {
            Session.Save(categorySetting);
        }

        public void InsertSetting(Setting setting)
        {
            Session.Save(setting);
        }
    }
}