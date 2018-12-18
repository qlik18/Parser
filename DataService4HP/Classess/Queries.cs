using System.Text;
using System;
using System.Linq;

public class Queries
{

    #region parametrized queries

    private static readonly string _EventParamForFormByEventMove = "Select ep.EventId,ISNULL(ep.ParamGroupId,0) as ParamGroupId,ep.EventParamId,ep.Name,ep.Details, "+
                    "fdm.Width,fdm.Height,fdm.[Top],fdm.[Left],fdm.LabelTop,fdm.LabelLeft,ept.TechName,ept.FriendlyName " +
                    ",ISNULL(ws.SourceId,0),ISNULL(ds.Name,'NULL'),ISNULL(ds.Content,'NULL'),ISNULL(ds.[Description],'NULL'),ISNULL(dst.Name,'NULL'),ep.isRequired,ep.BoundEventParamId " +
                    "From {0}.dbo.EventParam ep " +
                    "join {0}.dbo.FormModellerMetaData fdm on fdm.EventParamId=ep.EventParamId " +
                    "join {0}.dbo.EventParamType ept on ep.EventParamTypeId=ept.EventParamTypeId " +
                    "left join {0}.dbo.ProcessSource ps on ps.ProcessSourceId=ep.DataSourceId "+
                    "left join {0}.dbo.WFSSource ws on ws.WFSSourceId=ps.WFSSourceId "+
                    "left join {1}.dbo.DataSource ds on ds.DataSourceId=ws.SourceId "+
                    "left join {1}.dbo.DataSourceType dst on dst.DataSourceTypeId=ds.DetailedDataSourceTypeId "+
                    "where ep.EventId =( " +
                    "Select e.EventId From {0}.dbo.Event e " +
                    "join {0}.dbo.EventMove em on em.EventId=e.EventId " +
                    "where em.EventMoveId=@EventMoveId)order by ep.ParamGroupId,  fdm.[Top], fdm.[Left], ep.[Order], ep.EventParamId--order by EventParamId";

    private static readonly string _BoundEventParamForIssue =
    "SELECT [EventParamId],[Value],ISNULL([DBValue],'-1'),ISNULL([DBExtValue],'NULL') " +
                    "FROM [EventParamValue] " +
                    "WHERE issueId=@IssueId AND EventParamId in ({0}) AND DockNr=0";

    private static readonly string _AllBoundEventParamForIssue =
    "SELECT [EventParamId],[Value],ISNULL([DBValue],'-1'),ISNULL([DBExtValue],'NULL') " +
                    "FROM [EventParamValue] " +
                    "WHERE issueId=@IssueId AND DockNr=0 AND RN = 1";

    private static readonly string _BillingBoundEventParamForIssue =
    "SELECT [EventParamId],[Value],ISNULL([DBValue],'-1'),ISNULL([DBExtValue],'NULL') " +
                    "FROM [EventParamValue] " +
                    "WHERE issueId=@IssueId AND EventParamId in ({0}) AND RN = 1";
    
    public static string EventParamForFormByEventMove(string WFSDB_Database, string DSDB_Database)
    {
        return string.Format(_EventParamForFormByEventMove, WFSDB_Database, DSDB_Database);
    }

    public static string BoundEventParamForIssue(string tmpString)
    {
        return string.Format(_BoundEventParamForIssue, tmpString);
    }

    public static string AllBoundEventParamForIssue()
    {
        return string.Format(_AllBoundEventParamForIssue);
    }

    public static string BillingBoundEventParamForIssue(string tmpString)
    {
        return string.Format(_BillingBoundEventParamForIssue, tmpString);
    }
    
    public static readonly string AUTHENTICATEUSER = "SELECT UserId FROM [dbo].[User] "+
	"WHERE [Login] = @Login AND TypeFlag = 0  AND (IsBlocked = 0 OR IsBlocked is NULL)";
    //public static string AuthenticateUser(string login)
    //{
    //    return string.Format(_authenticateUser, login);
    //}
    #endregion


    #region Helios Users
    public static readonly string ALL_USERS = "SELECT * FROM Users order by surname";
    #endregion

}