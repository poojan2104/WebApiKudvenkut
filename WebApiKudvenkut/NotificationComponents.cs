using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApiKudvenkut.Models;

namespace WebApiKudvenkut
{
    public class NotificationComponents
    {
        //public void RegisterNotification(DateTime currentTime)
        //{
        //    string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
        //    string sqlCommand = @"SELECT [ContactId],[ContactName],[ContactNumber] from [Contects] where [AddedOn] > @AddedOn";
        //    using (SqlConnection con = new SqlConnection(conStr))
        //    {
        //        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        //        cmd.Parameters.AddWithValue("@AddedOn", currentTime);
        //        if (con.State != System.Data.ConnectionState.Open)
        //        {
        //            con.Open();
        //        }
        //        cmd.Notification = null;
        //        SqlDependency sqlDep = new SqlDependency(cmd);
        //        sqlDep.OnChange += sqlDep_OnChange;


        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            // nothing add to here now
        //        }
        //    }
        //}

        //private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    if (e.Type == SqlNotificationType.Change)
        //    {
        //        SqlDependency sqlDep = sender as SqlDependency;
        //        sqlDep.OnChange -= sqlDep_OnChange;

        //        //here we will send notification message to client
        //        var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        //        notificationHub.Clients.All.notify("added");

        //        //re register notification
        //        RegisterNotification(DateTime.Now);
        //    }
        //}

        //public List<Contect> GetContacts(DateTime afterDate)
        //{
        //    using (MyPushNotificationEntities db = new MyPushNotificationEntities())
        //    {
        //        return db.Contects.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();
        //    }
        //} 
    }
}