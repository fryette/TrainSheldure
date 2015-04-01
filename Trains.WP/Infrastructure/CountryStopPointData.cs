using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Trains.Model.Entities;

namespace Trains.WP.Infrastructure
{
  public class CountryStopPointData
    {
        private static readonly CountryStopPointData MenuDataSource = new CountryStopPointData();

        private readonly ObservableCollection<CountryStopPointDataGroup> _groups = new ObservableCollection<CountryStopPointDataGroup>();

        public ObservableCollection<CountryStopPointDataGroup> Groups
        {
            get { return _groups; }
        }

        public static async Task<IEnumerable<CountryStopPointDataGroup>> GetGroupsAsync()
        {
            await MenuDataSource.GetMenuDataAsync();

            return MenuDataSource.Groups;
        }

        public static async Task<CountryStopPointDataGroup> GetGroupAsync(string uniqueId)
        {
            await MenuDataSource.GetMenuDataAsync();
            var matches = MenuDataSource.Groups.Where(group => group.UniqueId.Equals(uniqueId));
            var menuDataGroups = matches as IList<CountryStopPointDataGroup> ?? matches.ToList();
            return menuDataGroups.Count() == 1 ? menuDataGroups.First() : null;
        }

        public static async Task<IList<CountryStopPointDataItem>> GetItemsAsync()
        {
            await MenuDataSource.GetMenuDataAsync();
            var matches =
                MenuDataSource.Groups.SelectMany(group => group.Items);
            var menuDataItems = matches as IList<CountryStopPointDataItem> ?? matches.ToList();
            return menuDataItems;
        }

        public static async Task<CountryStopPointDataItem> GetItemByIdAsync(string itemId)
        {
            await MenuDataSource.GetMenuDataAsync();
            var matches =
                MenuDataSource.Groups.SelectMany(group => group.Items);
            var menuDataItems = matches as IList<CountryStopPointDataItem> ?? matches.ToList();
            return menuDataItems.First(x => x.UniqueId == itemId);
        }

        private async Task GetMenuDataAsync()
        {
            if (_groups.Count != 0)
                return;

            var dataUri = new Uri("ms-appx:///Trains.Model/DataModel/StopPointsru.json");

            var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var jsonText = await FileIO.ReadTextAsync(file);
            var jsonObject = JsonObject.Parse(jsonText);
            var jsonArray = jsonObject["Groups"].GetArray();

            foreach (var groupValue in jsonArray)
            {
                var groupObject = groupValue.GetObject();
                var group = new CountryStopPointDataGroup(groupObject["UniqueId"].GetString(),
                    groupObject["Title"].GetString());

                foreach (var itemObject in groupObject["Items"].GetArray().Select(itemValue => itemValue.GetObject()))
                {
                    group.Items.Add(new CountryStopPointDataItem(itemObject["UniqueId"].GetString(), itemObject["Country"].GetString(), itemObject["Exp"].GetString()));
                }
                Groups.Add(group);
            }
        }
    }
}
