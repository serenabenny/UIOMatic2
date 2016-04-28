using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Example
{
    public class Startup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //var ctx = applicationContext.DatabaseContext;
            //var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger, ctx.SqlSyntax);

            //if (!db.TableExist("Redemption"))
            //    db.CreateTable<Redemption>();
            //if (!db.TableExist("RedemptionRecord"))
            //    db.CreateTable<RedemptionRecord>();
        }
    }
}