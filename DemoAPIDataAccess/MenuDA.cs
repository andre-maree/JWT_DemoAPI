using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;
using Models;
using Microsoft.Extensions.Configuration;

namespace DemoAPIDataAccess
{
    public class MenuDA : BaseDA, IMenuDA
    {
        //private IConfiguration _config;

        public MenuDA(IConfiguration config) : base(config) {}

        public async Task<string> GetMenu()
        {
            IEnumerable<MenuItem> res = await ExecuteStoredProcedureQueryAsync<MenuItem>("[dbo].[SP_Get_Menu]");

            List<MenuItem> items = res.ToList();

            IEnumerable<MenuItem> topitems = res.Where(i => i.ParentId == -1);

            foreach (MenuItem? item in topitems)
            {
                List<MenuItem> subs = items.Where(i => i.ParentId == item.Id).ToList();

                RecurseAddChildren(items, subs, item);
            }

            string json = JsonConvert.SerializeObject(topitems,
                        Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new IgnorePropertiesResolver("Id", "ParentId")
                        });

            return json;
        }

        private static void RecurseAddChildren(List<MenuItem> items, List<MenuItem> children, MenuItem parent)
        {
            if (children.Count > 0)
            {
                parent.Children = new();
                parent.Children.AddRange(children);
            }

            foreach (MenuItem item in children)
            {
                List<MenuItem> subs = items.Where(i => i.ParentId == item.Id).ToList();

                RecurseAddChildren(items, subs, item);
            }
        }
    }

    public class IgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly HashSet<string> _ignoreProps;
        public IgnorePropertiesResolver(params string[] propNamesToIgnore)
        {
            _ignoreProps = new HashSet<string>(propNamesToIgnore);
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (_ignoreProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }
            return property;
        }
    }
}
