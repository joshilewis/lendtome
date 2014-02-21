'use strict';

/* Controllers */

var lendtomeControllers = angular.module('lendtomeControllers', []);

lendtomeControllers.controller('userItemsController', ['$scope', 'userItems', '$route',
  function ($scope, userItems, $route) {
      $scope.userItems = userItems.query();

      $scope.addNew = function() {
          userItems.save($scope.newUserItem);
          $route.reload();
      };

      $scope.delete = function(userItem) {
          userItems.delete({ ownershipId: userItem.id });
          $route.reload();
      };

  }]);