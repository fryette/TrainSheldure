starter.controller("homeController",
["$scope", "dataService", "stations",
function homeController($scope, dataService, stations) {
  var today = new Date();
  $scope.dt = today;
  $scope.stations = stations;
  $scope.trains = [];
  $scope.isLoading = false;

  // Disable selection
  $scope.disabled = function (date) {
    return date < today;
  };

  $scope.find = find;
  $scope.getMatches = getMatches;

  function find() {
    var fromItem = $scope.fromItem
      , toItem = $scope.toItem
      , date = $scope.dt;

    if (!fromItem || !toItem) {
      console.log('station error')
      return;
    }

    $scope.trains = [];
    $scope.isLoading = true;

    dataService.findTrains(fromItem.Ecp, toItem.Ecp, date).then(function (data) {
      $scope.trains = data.data.filter(function (train) {
        var startTime = new Date(train.StartTime);
        var now = Date.now();
        return startTime > now;
      });
      $scope.isLoading = false;
    });
  }

  function getMatches(text) {
    return text ? stations.filter(function (station) {
      var lowercaseText = angular.lowercase(text)
        , lowercaseStationName = angular.lowercase(station.Value);
      return lowercaseStationName.indexOf(lowercaseText) === 0;
    }) : stations;
  }
}]);