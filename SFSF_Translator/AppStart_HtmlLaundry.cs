using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(CloudTranslator.AppStart_HtmlLaundry), "Start")]
namespace CloudTranslator
{
    public class AppStart_HtmlLaundry
    {

            public static void Start() 
            { 
                ModelBinders.Binders.DefaultBinder = new NetAcademia.Web.Mvc.HtmlLaundryModelBinder("default");
				
				//use the line below if you did not pack your html capable strings into a model but use them as action method string parametrs
				//ModelBinders.Binders.DefaultBinder = new NetAcademia.Web.Mvc.HtmlLaundryModelBinder("default") { TryToResolveStringParameterAttributes = true };
            }
    }
}

