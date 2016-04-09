using System.Text;
using Movselex.Core.Resources;

namespace Movselex.Core.Models
{

// ReSharper disable once InconsistentNaming
    internal class MovselexSQLBuilder
    {

        /// <summary>
        /// ライブラリを取得するSQLを生成します。
        /// </summary>
        /// <returns></returns>
        public string CreateSelectLibrary(LibraryMode libraryMode, LibraryCondition libCondition)
        {
            bool isFullSql = false;
            var sb = new StringBuilder();
            sb.Append(SQLResource.SelectLibraryList);
            sb.Append(" WHERE ");
            sb.Append(CreateRatingWhereString(libraryMode));

            if (!string.IsNullOrEmpty(libCondition.FilteringText))
            {
                // フィルタリング
                var filterString = libCondition.FilteringText.ToLower();
                sb.Append(" AND ");
                var normalFilter = "lower(IFNULL(FILEPATH,'') || IFNULL(TITLE,'') || IFNULL(GPL.GROUPNAME,'') || IFNULL(SEASON,'')) LIKE '%" +
                        EscapeSQL(filterString) + "%' ";
                if (filterString.Length < 3)
                {
                    sb.Append(normalFilter);
                }
                else
                {
                    var migemoFilter = "ISMATCHMIGEMO('" + EscapeSQL(filterString) +
                                       "',lower(IFNULL(FILEPATH,'') || IFNULL(TITLE,'') || IFNULL(GPL.GROUPNAME,''))) ";
                    sb.Append(string.Format("(({0}) OR ({1}))", normalFilter, migemoFilter));
                }
            }

            var condSql = libCondition.Condition.Sql;

            switch (libCondition.FilteringMode)
            {
                case FilteringMode.SQL:
                    condSql = condSql.ToUpper();
                    if (string.IsNullOrEmpty(condSql)) break;
                    if (!(condSql.Substring(0, 3).Equals("AND")
                          || condSql.Substring(0, 2).Equals("OR"))) isFullSql = true;
                    sb.Append(condSql);
                    break;

                case FilteringMode.Group:
                    // TODO: Gidでもよいのでは？
                    sb.Append(" AND ");
                    if (!string.IsNullOrEmpty(condSql))
                    {
                        sb.Append(" GPL.GROUPNAME = '" + EscapeSQL(condSql) + "'");
                    }
                    else
                    {
                        sb.Append(" GPL.GROUPNAME IS NULL ");
                    }
                    sb.Append(" ORDER BY round(PL.NO)");
                    break;
            }
            

            var sql = sb.ToString();
            if (isFullSql) sql = SQLResource.SelectLibraryList + " " + condSql;
            if (!sql.ToUpper().Contains("ORDER BY")) sql += " ORDER BY PL.DATE desc ";
            if (libCondition.Condition.IsLimited && libCondition.MaxLimitNum > 0) sql += string.Format(" LIMIT {0}", libCondition.MaxLimitNum);
            return sql;
        }

        /// <summary>
        /// グループを取得するSQLを生成します。
        /// </summary>
        /// <param name="libraryMode"></param>
        /// <param name="lastSql"></param>
        /// <returns></returns>
        public string CreateSelectGroup(LibraryMode libraryMode, string lastSql)
        {
            var sql = SQLResource.SelectGroupList;
            var join = new StringBuilder();

            switch (libraryMode)
            {
                case LibraryMode.Normal:
                    join.Append("LEFT JOIN (SELECT PPL.GID , count(*) cnt ");
                    join.Append("FROM MOVLIST PPL ");
                    join.Append("INNER JOIN  MOVLIST SPL ON PPL.ID = SPL.ID  AND ");
                    join.Append("SPL.RATING = 9");
                    join.Append(" GROUP BY PPL.GID ");
                    join.Append(") CL ON ifnull(PL.GID,'') = ifnull(CL.GID,'') ");
                    join.Append("LEFT JOIN (SELECT PPPL.GID , count(*) cnt ");
                    join.Append("FROM MOVLIST PPPL WHERE ");
                    join.Append(CreateRatingWhereString(libraryMode));
                    join.Append("GROUP BY PPPL.GID ");
                    join.Append(") ACL ON ifnull(PL.GID,'') = ifnull(ACL.GID,'') ");
                    join.Append("LEFT JOIN  MOVGROUPLIST GPL ON PL.GID = GPL.GID ");
                    sql = sql.Replace("#CLASS1COUNT#", ",ifnull(CL.cnt,0) ");
                    break;
                case LibraryMode.Favorite:
                case LibraryMode.Exclude:
                    join.Append("LEFT JOIN (SELECT PPPL.GID , count(*) cnt ");
                    join.Append("FROM MOVLIST PPPL WHERE ");
                    join.Append(CreateRatingWhereString(libraryMode));
                    join.Append(" GROUP BY PPPL.GID ");
                    join.Append(") ACL ON PL.GID = ACL.GID ");
                    join.Append("LEFT JOIN  MOVGROUPLIST GPL ON PL.GID = GPL.GID ");
                    if (libraryMode == LibraryMode.Favorite) sql = sql.Replace("#CLASS1COUNT#", ",CNT ");
                    if (libraryMode == LibraryMode.Exclude) sql = sql.Replace("#CLASS1COUNT#", ",'' ");
                    break;
            }

            sql = sql.Replace("#JOIN#", join.ToString());
            sql = sql.Replace("#LASTEXECSQL#", lastSql);

            return sql;
        }


        /// <summary>
        /// RATING条件を生成します。
        /// </summary>
        /// <param name="libraryMode"></param>
        /// <returns></returns>
        private string CreateRatingWhereString(LibraryMode libraryMode)
        {
            switch (libraryMode)
            {
                case LibraryMode.Normal:
                    return "RATING > 0 ";
                case LibraryMode.Favorite:
                    return "RATING = 9 ";
                case LibraryMode.Exclude:
                    return "RATING = 0 ";
            }
            return "";
        }

        public static string EscapeSQL(string sql)
        {
            return sql.Replace("'", "''");
        }

        public string CreateSelectNowPlaying()
        {
            var sb = new StringBuilder();

            sb.Append(SQLResource.SelectLibraryList);

            sb.Append(" INNER JOIN ");
            sb.Append(" PLAYINGLIST ");
            sb.Append("PIL ON PL.ID = PIL.ID ");
            sb.Append("ORDER BY PIL.SORT");

            return sb.ToString();
        }
    }
}