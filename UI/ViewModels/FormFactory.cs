namespace UI.ViewModels
{
	public interface IFormFactory
	{
		Form Create();
	}

	public class FormFactory : IFormFactory
	{
		public Form Create()
		{
			return new Form();
		}
	}
}