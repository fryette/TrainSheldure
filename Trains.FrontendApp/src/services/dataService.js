angular.module('starter')
.factory('dataService',
["$http", "$q",
  function ($http, $q) {

    function findTrains(from, to, data) {
      var defered = $q.defer();

      var config = {
        from: from,
        to: to,
        date: data
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