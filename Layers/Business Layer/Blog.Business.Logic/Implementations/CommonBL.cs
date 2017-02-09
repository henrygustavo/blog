namespace Blog.Business.Logic.Implementations
{
	using DataAccess.Interfaces;
	using Interfaces;
	using System.Collections.Generic;
	using Entity;

	public class CommonBL: ICommonBL
	{
		private readonly ICommonRepository _repository;

		public CommonBL(ICommonRepository repository)
		{
			_repository = repository;
		}

		public System.DateTime GetCurrentDateTime()
		{
			return _repository.GetCurrentDateTime();
		}

		public List<Setting> GetByIdCategorySetting(int idCategorySetting)
		{
          return _repository.GetByIdCategorySetting(idCategorySetting);
		}

		public Setting GetSetting(int idSetting)
		{
          return _repository.GetSetting(idSetting);
		}
		
		public void InsertCategorySetting(CategorySetting categorySetting)
        {
            _repository.InsertCategorySetting(categorySetting);
        }

        public void InsertSetting(Setting setting)
        {
            _repository.InsertSetting(setting);
        }
	}
}