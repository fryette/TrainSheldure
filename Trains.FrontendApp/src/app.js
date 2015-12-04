var starter = angular.module('starter', ["ngAnimate", 'ngMaterial', "templates", "ui.bootstrap", "ui.router"]);

starter.config(
  ["$stateProvider", "$urlRouterProvider", "$locationProvider",
    function ($stateProvider, $urlRouterProvider, $locationProvider) {

      $urlRouterProvider.otherwise("/home");

      $stateProvider
        .state('home', {
          url: '/home',
          controller: 'homeController',
          templateUrl: 'pages/home/home.html',
          resolve: {
            stations: ["dataService", function (dataService) {
              return dataService.getStations().then(function (data) {
                return data.data["Беларусь"];
              });
            }]
          }
        })

      $locationProvider.html5Mode(true);
    }]);