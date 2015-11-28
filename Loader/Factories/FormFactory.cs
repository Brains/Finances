using UI.ViewModels;

namespace Loader.Factories
{
	public class FormFactory : IFormFactory
	{
		public Form Create()
		{
			return new Form();
		}
	}
}