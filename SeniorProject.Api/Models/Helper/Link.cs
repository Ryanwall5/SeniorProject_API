using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Helper
{
    //public class Link
    //{
    //    public const string GetMethod = "Get";

    //    public static Link To(string routeName, object routeValues = null)
    //        => new Link
    //        {
    //            RouteName = routeName,
    //            RouteValues = routeValues,
    //            Method = GetMethod,
    //            Relations = null
    //        }; 

    //    [JsonProperty(Order = -4)]
    //    public string Href { get; set; }

    //    [JsonProperty(Order = -3, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
    //    [DefaultValue(GetMethod)]
    //    public string Method { get; set; }

    //    [JsonProperty(Order = -2, PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore)]
    //    public string[] Relations { get; set; }

    //    [JsonIgnore]
    //    public string RouteName { get; set; }

    //    [JsonIgnore]
    //    public object RouteValues { get; set; }
    //}
    public class Link
    {
        public Link(string href, string rel, string method)
        {
            Href = href;
            Relation = rel;
            Method = method;
        }
        public string Href { get; set; }
        public string Relation { get; set; } 
        public string Method { get; set; }
    }
}
