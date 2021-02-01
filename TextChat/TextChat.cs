namespace TextChat
{
    using Events;
    using Exiled.API.Features;
    using System;

    public class TextChat : Plugin<Config>
    {
		private static readonly Lazy<TextChat> LazyInstance = new Lazy<TextChat>(() => new TextChat());

		internal RoundHandler RoundHandler { get; private set; }
		internal PlayerHandler PlayerHandler { get; private set; }

		public static TextChat Instance => LazyInstance.Value;

		private TextChat()
        {

        }

		public override void OnEnabled()
		{
			RegisterEvents();

			Database.Open();

			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			UnregisterEvents();

			Database.Close();

			base.OnDisabled();
		}

		private void RegisterEvents()
		{
			RoundHandler = new RoundHandler();
			PlayerHandler = new PlayerHandler();

			Exiled.Events.Handlers.Server.RestartingRound += RoundHandler.OnRestartingRound;

			Exiled.Events.Handlers.Player.Verified += PlayerHandler.OnVerified;
			Exiled.Events.Handlers.Player.Destroying += PlayerHandler.OnDestroying;
		}

		private void UnregisterEvents()
		{
			Exiled.Events.Handlers.Server.RestartingRound -= RoundHandler.OnRestartingRound;

			Exiled.Events.Handlers.Player.Verified -= PlayerHandler.OnVerified;
			Exiled.Events.Handlers.Player.Destroying -= PlayerHandler.OnDestroying;

			RoundHandler = null;
			PlayerHandler = null;
		}
	}
}
