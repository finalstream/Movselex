using System;
using Firk.Database.SQLiteFunctions;
using Movselex.Core.Models.SQLiteFunctions;

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

        /// <summary>
        /// SQLite日時フォーマット。
        /// </summary>
        public const string SqliteDateTimeFormat = "yyyy-MM-dd HH:mm:ss";


        public const string TimeFormatHourMinuteSecond = "H:mm:ss";


        public const string TimeFormatMinuteSecond = "m:ss";

        public const string TimeEmptyString = "00:00";

        public const string VideoSizeFormat = "{0}x{1}";

        public const string SeasonFormat = "{0} {1}";

        public const int CurrentSchemaVersion = 1;

        // ReSharper disable once InconsistentNaming
        public static readonly Type[] SupportSQLiteFunctions = new[]
        {
            typeof (SumStringSQLiteFunction),
            typeof (GetFileSizeSQLiteFunction),
            typeof (IsMatchMigemoSQLiteFunction),
            typeof (GetDirectoryPathSQLiteFunction)
        };
    }
}