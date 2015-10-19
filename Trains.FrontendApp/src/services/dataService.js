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

    return {
      findTrains: findTrains
    };
  }]);