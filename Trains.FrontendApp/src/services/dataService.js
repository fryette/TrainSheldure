angular.module('starter')
.factory('dataService',
["$http", "$q",
  function ($http, $q) {

    function findTrains(fromEcp, toEcp, date) {
      var defered = $q.defer();
      var config = {
        params: {
          fromEcp: fromEcp,
          toEcp: toEcp,
          date: date
        }
      };
      $http.get('/api/Train', config).then(function (data) {
        defered.resolve(data);
      });
      return defered.promise;
    }

    function getStations() {
      return $http.get('wwwroot/stations.json');
    }

    return {
      findTrains: findTrains,
      getStations: getStations
    };
  }]);