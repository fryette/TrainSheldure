var starter = angular.module('starter', ["ui.router", "templates"]);

starter.config(
  ["$stateProvider", "$urlRouterProvider", "$locationProvider",
    function ($stateProvider, $urlRouterProvider, $locationProvider) {

      $urlRouterProvider.otherwise("/home");

      $stateProvider
        .state('home', {
          url: '/home',
          controller: 'homeController',
          templateUrl: 'pages/home/home.html'
        })

      $locationProvider.html5Mode(true);
    }]);