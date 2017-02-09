namespace Blog.Business.Entity
{
	public class Setting : Entity
	{ 

		public virtual string Name { get; set; }

		public virtual string ParamValue { get; set; }

		public virtual int IdCategorySetting { get; set; }
	}
}