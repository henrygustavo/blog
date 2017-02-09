namespace Blog.DataAccess.Interfaces
{
    using Business.Entity;
    using System;
	using System.Collections.Generic;

    public interface ICommonRepository : IRepository<Common>
    {
        DateTime GetCurrentDateTime();

		List<Setting> GetByIdCategorySetting(int idCategorySetting);

		Setting GetSetting(int idSetting);

		void InsertCategorySetting(CategorySetting categorySetting);

        void InsertSetting(Setting setting);
    }
}