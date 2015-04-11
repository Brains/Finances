using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;

namespace UI
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
	}
}
