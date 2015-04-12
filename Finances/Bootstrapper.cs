using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;

namespace Finances
{
	class Bootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell ()
		{
			return Shell;
//			return ServiceLocator.Current.GetInstance<Shell>();
		}

		//------------------------------------------------------------------
		protected override void InitializeShell ()
		{
			Application.Current.MainWindow = (Window) Shell;
			Application.Current.MainWindow.Show();
		}

		//------------------------------------------------------------------
		protected override void ConfigureModuleCatalog ()
		{
//			Type tracker = typeof(TrackerModule);
//
//			ModuleCatalog.AddModule(
//			  new ModuleInfo()
//			  {
//				  ModuleName = tracker.Name,
//				  ModuleType = tracker.AssemblyQualifiedName,
//			  });
		}
	}
}
