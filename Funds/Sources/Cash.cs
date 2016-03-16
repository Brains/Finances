using Common;

namespace Funds.Sources
{
	public class Cash : Base
	{
		private readonly ISettings settings;

		public Cash(ISettings settings)
		{
			this.settings = settings;
			Name = "Cash";

			Update += () => settings.Save("Cash", Value);
		}

		public override void PullValue()
		{
			Value = decimal.Parse(settings.Cash);
		}
	}
}