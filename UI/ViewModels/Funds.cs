using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using UI.Interfaces;
using UI.Services;

namespace UI.ViewModels
{
	public class Funds : PropertyChangedBase, IViewModel
	{
		public Funds(IFundsSource[] sources)
		{
			var values = sources.Select(source => source.Get());
		}
	}
}