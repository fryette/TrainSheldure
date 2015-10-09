starter.controller("homeController",
["$scope", "dataService",
function homeController($scope, dataService) {
  $scope.result = "Hello, World!";
  dataService.findTrains("Брест", "Пинск", new Date(2015, 9, 15)).then(onFindsuccess);

  function onFindsuccess(data) {
    $scope.result = data;
  }

}]);