using Common;

namespace Funds
{
	public class SavedSource : FundsSource
	{
		private readonly ISettings settings;
		public SavedSource(ISettings settings)
		{
			this.settings = settings;
			Name = "SavedSource";
        }

		public override void PullValue()
		{
			Value = decimal.Parse(settings.Cash);
		}
	}
}