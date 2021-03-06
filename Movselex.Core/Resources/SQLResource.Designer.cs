﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Movselex.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SQLResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SQLResource() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Movselex.Core.Resources.SQLResource", typeof(SQLResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   DELETE FROM MOVGROUPLIST 
        ///WHERE GID = @Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DeleteGroup {
            get {
                return ResourceManager.GetString("DeleteGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   DELETE FROM MOVLIST 
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DeleteLibrary {
            get {
                return ResourceManager.GetString("DeleteLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   DELETE FROM PLAYINGLIST に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DeletePlayingList {
            get {
                return ResourceManager.GetString("DeletePlayingList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   INSERT INTO MOVGROUPLIST(
        ///  GROUPNAME,
        ///  KEYWORD,
        ///  LASTUPDATE
        ///) VALUES (
        ///  @GroupName,
        ///  @Keyword,
        ///  @LastUpdate
        ///) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InsertGroup {
            get {
                return ResourceManager.GetString("InsertGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   INSERT INTO MOVLIST(
        ///  FILEPATH,
        ///  TITLE,
        ///  NO,
        ///  GID,
        ///  LENGTH,
        ///  CODEC,
        ///  RATING,
        ///  VIDEOSIZE,
        ///  PLAYCOUNT,
        ///  DATE,
        ///  NOTFOUND,
        ///  OPTION,
        ///  TAG,
        ///  ADDDATE,
        ///  LASTPLAYDATE,
        ///  DRIVE,
        ///  FILESIZE,
        ///  PLAYED,
        ///  SEASON
        ///) VALUES (
        ///  @FilePath,
        ///  @Title,
        ///  @No,
        ///  @Gid,
        ///  @Length,
        ///  @Codec,
        ///  @Rating,
        ///  @VideoSize,
        ///  @PlayCount,
        ///  @Date,
        ///  @NotFound,
        ///  @Option,
        ///  @Tag,
        ///  @AddDate,
        ///  @LastPlayDate,
        ///  SUBSTR(@FilePath,1,1),
        ///  @Filesize,
        ///  @Played,
        ///  @Season
        ///) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InsertLibrary {
            get {
                return ResourceManager.GetString("InsertLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   INSERT INTO PLAYINGLIST(ID, SORT) VALUES (@Id, @Sort) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InsertPlayingList {
            get {
                return ResourceManager.GetString("InsertPlayingList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT FILEPATH FROM MOVLIST GROUP BY FILEPATH に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectAllFilePath {
            get {
                return ResourceManager.GetString("SelectAllFilePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT count(ID)
        ///FROM MOVLIST
        ///WHERE GID = @Gid
        ///AND RATING = 9 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectFavGroupIdCount {
            get {
                return ResourceManager.GetString("SelectFavGroupIdCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///  GID
        ///FROM MOVGROUPLIST 
        ///WHERE lower(GROUPNAME) = @GroupName に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectGIdByGroupName {
            get {
                return ResourceManager.GetString("SelectGIdByGroupName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT count(GID) GCNT 
        ///FROM MOVLIST
        ///WHERE GID = @Gid
        ///GROUP BY GID に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectGroupIdCount {
            get {
                return ResourceManager.GetString("SelectGroupIdCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///  GID,
        ///  GROUPNAME
        ///FROM MOVGROUPLIST 
        ///WHERE KEYWORD = @Keyword に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectGroupKeyword {
            get {
                return ResourceManager.GetString("SelectGroupKeyword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT GGPL.GID,GNAME GroupName,CNT Count,cast(FAVCNT as integer) FavoriteCount, DL.DRIVE, FSL.FILESIZE, GGPL.KEYWORD, GGPL.COMPLETE IsCompleted, GGPL.LASTUPDATE
        ///FROM (SELECT GPL.GID,GPL.GROUPNAME GNAME , ifnull(ACL.cnt,0) CNT #CLASS1COUNT# FAVCNT, GPL.KEYWORD, GPL.COMPLETE, GPL.LASTUPDATE
        ///FROM (#LASTEXECSQL#) PL
        ///#JOIN#
        ///GROUP BY GPL.GROUPNAME
        ///ORDER BY GPL.LASTUPDATE DESC, GPL.GID DESC) GGPL
        ///LEFT JOIN (SELECT DSLIST.GID, SUMSTR(DSLIST.DRIVE) DRIVE FROM MOVLIST DSLIST GROUP BY DSLIST.GID) DL ON ifnull(G [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectGroupList {
            get {
                return ResourceManager.GetString("SelectGroupList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT ID FROM MOVLIST WHERE FILEPATH LIKE @FileName  に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectIdFromFileName {
            get {
                return ResourceManager.GetString("SelectIdFromFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///    PL.ID
        ///FROM MOVLIST PL 
        ///WHERE GID = @Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectIdFromGid {
            get {
                return ResourceManager.GetString("SelectIdFromGid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT ID, FILEPATH FROM MOVLIST WHERE LENGTH = @Length OR ifnull(TITLE, &apos;&apos;) = &apos;&apos; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectInCompleteIdFilePaths {
            get {
                return ResourceManager.GetString("SelectInCompleteIdFilePaths", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT last_insert_rowid() AS LASTROWID に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectLastInsertRowid {
            get {
                return ResourceManager.GetString("SelectLastInsertRowid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///  PL.ID AS ID,
        ///  FILEPATH,
        ///  GPL.GROUPNAME AS GROUPNAME,
        ///  TITLE,
        ///  NO,
        ///  LENGTH,
        ///  CODEC,
        ///  PLAYED AS ISPLAYED,
        ///  RATING,
        ///  DATE,
        ///  VIDEOSIZE,
        ///  PLAYCOUNT,
        ///  ADDDATE,
        ///  LASTPLAYDATE,
        ///  GPL.GID AS GID,
        ///  SEASON
        ///FROM MOVLIST PL 
        ///LEFT JOIN MOVGROUPLIST GPL
        ///ON PL.GID = GPL.GID に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectLibraryList {
            get {
                return ResourceManager.GetString("SelectLibraryList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT (SELECT ID FROM MOVLIST WHERE GID = @Gid AND ABS(NO) &lt; ABS(@No) ORDER BY ABS(NO) DESC Limit 1) Previous,
        ///(SELECT ID FROM MOVLIST WHERE GID = @Gid AND ABS(NO) &gt; ABS(@No) ORDER BY ABS(NO) ASC Limit 1) Next に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectLibraryPreviousAndNextId {
            get {
                return ResourceManager.GetString("SelectLibraryPreviousAndNextId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT GETDIRPATH(FILEPATH) FP, COUNT(*) CNT FROM MOVLIST GROUP BY FP ORDER BY CNT DESC に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectMostUseDirectoryPath {
            get {
                return ResourceManager.GetString("SelectMostUseDirectoryPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT * FROM (#LastExecSql#) ORDER BY random() limit @LimitNum に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SelectShuffleLibrary {
            get {
                return ResourceManager.GetString("SelectShuffleLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///  COUNT(ID) 
        ///FROM MOVLIST 
        ///WHERE FILEPATH = :FilePath に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL002 {
            get {
                return ResourceManager.GetString("SQL002", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE PLAYLIST 
        ///SET
        ///TITLE = :Title ,
        ///PLAYCOUNT = :PlayCount ,
        ///RATING = :Rating ,
        ///DATE = :Date,
        ///ADDDATE = :AddDate,
        ///NOTFOUND = :NotFound
        ///WHERE ID = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL005 {
            get {
                return ResourceManager.GetString("SQL005", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST 
        ///SET
        ///LASTPLAYDATE = :LastPlayDate
        ///WHERE ID = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL007 {
            get {
                return ResourceManager.GetString("SQL007", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT sum(round((julianday(&apos;00:&apos; || length)-julianday(&apos;00:00:00&apos;)) * 86400))
        ///FROM PLAYLIST PL に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL016 {
            get {
                return ResourceManager.GetString("SQL016", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///  PLAYCOUNT 
        ///FROM PLAYLIST 
        ///WHERE ID = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL017 {
            get {
                return ResourceManager.GetString("SQL017", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT count(*) FROM PLAYINGLIST に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL020 {
            get {
                return ResourceManager.GetString("SQL020", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///    ID,
        ///    TITLE
        ///FROM MOVLIST
        ///WHERE GID IS NULL に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL028 {
            get {
                return ResourceManager.GetString("SQL028", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET FILEPATH = :FilePath,
        ///TITLE = :Title,
        ///DRIVE = SUBSTR(:FilePath,1,1),
        ///FILESIZE = GETFILESIZE(:FilePath)
        ///WHERE ID = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL033 {
            get {
                return ResourceManager.GetString("SQL033", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///  KEYWORD
        ///FROM MOVGROUPLIST 
        ///WHERE GID = :Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL035 {
            get {
                return ResourceManager.GetString("SQL035", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVGROUPLIST 
        ///SET
        ///KEYWORD = :Keyword
        ///WHERE GID = :Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL036 {
            get {
                return ResourceManager.GetString("SQL036", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT max(sort) FROM PLAYINGLIST に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL037 {
            get {
                return ResourceManager.GetString("SQL037", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT GID FROM MOVLIST WHERE ID = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL039 {
            get {
                return ResourceManager.GetString("SQL039", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT MGL.GROUPNAME
        ///FROM MOVLIST ML inner join MOVGROUPLIST MGL ON ML.GID= MGL.GID 
        ///WHERE FILEPATH like :FilePath limit 1 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL042 {
            get {
                return ResourceManager.GetString("SQL042", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET TITLE = :Title
        ///WHERE ID = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL045 {
            get {
                return ResourceManager.GetString("SQL045", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET NO = :No
        ///WHERE ID = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL046 {
            get {
                return ResourceManager.GetString("SQL046", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT 
        ///  TITLE,
        ///  ID,
        ///  GID
        ///FROM MOVLIST
        ///WHERE FILEPATH like :FilePath
        ///ORDER BY ADDDATE DESC LIMIT 1 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL047 {
            get {
                return ResourceManager.GetString("SQL047", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT ID,
        ///FILEPATH
        ///FROM MOVLIST
        ///WHERE GID = :Gid
        ///AND ABS(NO) &gt; (SELECT ABS(NO) FROM MOVLIST WHERE ID = :Id)
        ///ORDER BY ABS(NO) ASC
        ///LIMIT 1 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL049 {
            get {
                return ResourceManager.GetString("SQL049", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT ID,
        ///FILEPATH
        ///FROM MOVLIST
        ///WHERE GID = :Gid
        ///AND ABS(NO) &lt; (SELECT ABS(NO) FROM MOVLIST WHERE ID = :Id)
        ///ORDER BY ABS(NO) DESC
        ///LIMIT 1 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL050 {
            get {
                return ResourceManager.GetString("SQL050", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT FILESIZE || ifnull(NO,&apos;&apos;) || LENGTH 
        ///FROM MOVLIST 
        ///GROUP BY FILESIZE || ifnull(NO,&apos;&apos;) || LENGTH に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL051 {
            get {
                return ResourceManager.GetString("SQL051", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SELECT ID,FILESIZE, NO, LENGTH 
        ///FROM MOVLIST 
        ///WHERE FILESIZE = :Filesize 
        ///AND ifnull(NO, &apos;&apos;) = :No 
        ///AND LENGTH = :Length 
        ///ORDER BY ID に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL052 {
            get {
                return ResourceManager.GetString("SQL052", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET FILEPATH = :FilePath,
        ///NOTFOUND = 0
        ///WHERE Id = :Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SQL053 {
            get {
                return ResourceManager.GetString("SQL053", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST 
        ///SET
        ///GID = @Gid
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateGidById {
            get {
                return ResourceManager.GetString("UpdateGidById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVGROUPLIST 
        ///SET
        ///GROUPNAME = @GroupName,
        ///KEYWORD = @Keyword
        ///WHERE GID = @Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateGroup {
            get {
                return ResourceManager.GetString("UpdateGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVGROUPLIST
        ///SET COMPLETE = @Complete
        ///WHERE GID = @Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateGroupIsCompleted {
            get {
                return ResourceManager.GetString("UpdateGroupIsCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVGROUPLIST 
        ///SET
        ///LASTUPDATE = @LastUpdate
        ///WHERE GID = @Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateGroupLastUpdateDatetime {
            get {
                return ResourceManager.GetString("UpdateGroupLastUpdateDatetime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET TITLE = @Title,
        ///NO = @No,
        ///SEASON = @Season
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateLibrary {
            get {
                return ResourceManager.GetString("UpdateLibrary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST 
        ///SET
        ///FILEPATH = @FilePath,
        ///DRIVE = SUBSTR(@FilePath,1,1),
        ///FILESIZE = GETFILESIZE(@FilePath)
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateLibraryFilePath {
            get {
                return ResourceManager.GetString("UpdateLibraryFilePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET PLAYED = @Played
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateLibraryIsPlayed {
            get {
                return ResourceManager.GetString("UpdateLibraryIsPlayed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET RATING = @Rating
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateLibraryRating {
            get {
                return ResourceManager.GetString("UpdateLibraryRating", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET TITLE = replace(TITLE, @OldGroupName, @NewGroupName)
        ///WHERE GID = @Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateLibraryReplaceGroupName {
            get {
                return ResourceManager.GetString("UpdateLibraryReplaceGroupName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET TITLE = @Title,
        ///LENGTH = @Length,
        ///CODEC = @Codec,
        ///VIDEOSIZE = @VideoSize
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateMediaInfo {
            get {
                return ResourceManager.GetString("UpdateMediaInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET PLAYCOUNT = PLAYCOUNT + 1,
        ///LASTPLAYDATE = @LastPlayDate
        ///WHERE ID = @Id に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdatePlayCount {
            get {
                return ResourceManager.GetString("UpdatePlayCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   UPDATE MOVLIST
        ///SET GID = null
        ///WHERE GID = @Gid に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UpdateUnGroup {
            get {
                return ResourceManager.GetString("UpdateUnGroup", resourceCulture);
            }
        }
    }
}
