namespace Master
{
    public static partial class Global
    {
        private static CoinMeter coinMeter;
        /// <summary>
        /// Set the <seealso cref="CoinMeter"/> script that controls how money is displayed on canvas.
        /// </summary>
        /// <param name="coinMeterController"><seealso cref="CoinMeter"/> that controls displayed money.</param>
        public static void SetCoinMeter(CoinMeter coinMeterController, int statingMoney)
        {
            coinMeter = coinMeterController;
            coinMeter.ManualUpdate(statingMoney);
        }

        /// <summary>
        /// Current money of the player.
        /// </summary>
        public static int money {
            get => coinMeter.Money;
            set => coinMeter.Money = value;
        }

        /// <summary>
        /// Amount of money required to win the game.
        /// </summary>
        public static int moneyToWin;

        /// <summary>
        /// Menu instance.
        /// </summary>
        public static Menu menu;

        /// <summary>
        /// Audio instance.
        /// </summary>
        public static AudioSystem audioSystem;
    }
}