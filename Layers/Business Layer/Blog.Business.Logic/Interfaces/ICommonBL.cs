namespace Blog.Business.Logic.Interfaces
{
    using System;
	using System.Collections.Generic;
	using Entity;
    public interface ICommonBL
    {
        DateTime GetCurrentDateTime();

		List<Setting> GetByIdCategorySetting(int idCategorySetting);

		Setting GetSetting(int idSetting);

		void InsertCategorySetting(CategorySetting categorySetting);

        void InsertSetting(Setting setting);
    }
}