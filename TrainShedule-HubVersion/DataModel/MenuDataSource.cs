using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace TrainShedule_HubVersion.DataModel
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class MenuDataItem
    {
        public MenuDataItem(String uniqueId, String title, String imagePath)
        {
            UniqueId = uniqueId;
            Title = title;
            ImagePath = imagePath;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string ImagePath { get; private set; }
        public bool IsEconom { get; set; }
        public bool SpecialSearch { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class MenuDataGroup
    {
        public MenuDataGroup(String uniqueId, String title)
        {
            UniqueId = uniqueId;
            Title = title;
            Items = new ObservableCollection<MenuDataItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public ObservableCollection<MenuDataItem> Items { get; private set; }

        public override string ToString()
        {
            return Title;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// 
    /// MenuDataSource initializes with data read from a static json file included in the 
    /// project.  This provides menu data at both design-time and run-time.
    /// </summary>
    public sealed class MenuDataSource
    {
        private static readonly MenuDataSource _menuDataSource = new MenuDataSource();

        private readonly ObservableCollection<MenuDataGroup> _groups = new ObservableCollection<MenuDataGroup>();

        public ObservableCollection<MenuDataGroup> Groups
        {
            get { return _groups; }
        }

        public static async Task<IEnumerable<MenuDataGroup>> GetGroupsAsync()
        {
            await _menuDataSource.GetMenuDataAsync();
            return _menuDataSource.Groups;
        }

        public static async Task<MenuDataGroup> GetGroupAsync(string uniqueId)
        {
            await _menuDataSource.GetMenuDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _menuDataSource.Groups.Where(group => group.UniqueId.Equals(uniqueId));
            var menuDataGroups = matches as IList<MenuDataGroup> ?? matches.ToList();
            return menuDataGroups.Count() == 1 ? menuDataGroups.First() : null;
        }

        public static async Task<MenuDataItem> GetItemAsync(string uniqueId)
        {
            await _menuDataSource.GetMenuDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches =
                _menuDataSource.Groups.SelectMany(group => group.Items).Where(item => item.UniqueId.Equals(uniqueId));
            var menuDataItems = matches as IList<MenuDataItem> ?? matches.ToList();
            return menuDataItems.Count() == 1 ? menuDataItems.First() : null;
        }

        private async Task GetMenuDataAsync()
        {
            if (_groups.Count != 0)
                return;

            var dataUri = new Uri("ms-appx:///DataModel/MenuData.json");

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
                    @group.Items.Add(new MenuDataItem(itemObject["UniqueId"].GetString(),
                        itemObject["Title"].GetString(),
                        itemObject["ImagePath"].GetString()));
                }
                Groups.Add(group);
            }
        }
    }
}