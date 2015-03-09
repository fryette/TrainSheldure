using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Trains.Model.Entities;

namespace Trains.Infrastructure.Infrastructure
{
    public class MenuData
    {
        private static readonly MenuData MenuDataSource = new MenuData();

        private readonly ObservableCollection<MenuDataGroup> _groups = new ObservableCollection<MenuDataGroup>();

        public ObservableCollection<MenuDataGroup> Groups
        {
            get { return _groups; }
        }

        public static async Task<IEnumerable<MenuDataGroup>> GetGroupsAsync()
        {
            await MenuDataSource.GetMenuDataAsync();

            return MenuDataSource.Groups;
        }

        public static async Task<MenuDataGroup> GetGroupAsync(string uniqueId)
        {
            await MenuDataSource.GetMenuDataAsync();
            var matches = MenuDataSource.Groups.Where(group => group.UniqueId.Equals(uniqueId));
            var menuDataGroups = matches as IList<MenuDataGroup> ?? matches.ToList();
            return menuDataGroups.Count() == 1 ? menuDataGroups.First() : null;
        }

        public static async Task<IEnumerable<MenuDataItem>> GetItemsAsync()
        {
            await MenuDataSource.GetMenuDataAsync();
            var matches =
                MenuDataSource.Groups.SelectMany(group => group.Items);
            var menuDataItems = matches as IList<MenuDataItem> ?? matches.ToList();
            return menuDataItems;
        }
        private async Task GetMenuDataAsync()
        {
            if (_groups.Count != 0)
                return;

            var dataUri = new Uri("ms-appx:///Trains.Model/DataModel/Menu.json");

            var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var jsonText = await FileIO.ReadTextAsync(file);
            var jsonObject = JsonObject.Parse(jsonText);
            var jsonArray = jsonObject["Groups"].GetArray();

            foreach (var groupValue in jsonArray)
            {
                var groupObject = groupValue.GetObject();
                var group = new MenuDataGroup(groupObject["UniqueId"].GetString(),
                    groupObject["Title"].GetString());

                foreach (var itemObject in groupObject["Items"].GetArray().Select(itemValue => itemValue.GetObject()))
                {
                    group.Items.Add(new MenuDataItem(itemObject["UniqueId"].GetString(),
                        itemObject["Title"].GetString(),
                        itemObject["ImagePath"].GetString(), itemObject["Description"].GetString()));
                }
                Groups.Add(group);
            }
        }
    }
}