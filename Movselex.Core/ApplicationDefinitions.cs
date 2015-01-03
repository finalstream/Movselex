namespace Movselex.Core
{
    public static class ApplicationDefinitions
    {
        /// <summary>
        ///     デフォルトのフィルタリングファイルパス（相対）。
        /// </summary>
        public const string DefaultFilteringFilePath = "sql\\default.json";

        /// <summary>
        ///     デフォルトのアプリ設定ファイルパス（相対）。
        /// </summary>
        public const string DefaultAppConfigFilePath = "config\\movselexconfig.json";

        /// <summary>
        ///     migemoディクショナリファイルパス（相対）。
        /// </summary>
        public const string MigemoDictionaryFilePath = "migemo\\dict\\migemo-dict";

        /// <summary>
        ///     データベースディレクトリ。
        /// </summary>
        public const string DatabaseDirectory = "database";
    }
}