'use strict';

/* Controllers */

var lendtomeControllers = angular.module('lendtomeControllers', []);

lendtomeControllers.controller('userItemsController', ['$scope', 'userItems', '$route', '$location',
  function ($scope, userItems, $route, $location) {
      $scope.userItems = userItems.query();

      $scope.delete = function(userItem) {
          userItems.delete({ ownershipId: userItem.id });
          $route.reload();
      };

      $scope.addFromIsbn = function () {
          $location.path('/addisbn/' + $scope.isbnNumber);
      };

  }]);



