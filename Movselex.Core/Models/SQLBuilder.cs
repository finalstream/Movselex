using System.Text;
using Movselex.Core.Resources;

namespace Movselex.Core.Models
{

// ReSharper disable once InconsistentNaming
    internal class SQLBuilder
    {
        /// <summary>
        /// ライブラリを取得するSQLを生成します。
        /// </summary>
        /// <returns></returns>
        public string CreateSelectLibrary()
        {
            var sql = SQLResource.SelectLibraryList;
            return sql + " ORDER BY PL.DATE desc ";
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
                    sql = sql.Replace("#CLASS1COUNT#", ",''");
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
    }
}