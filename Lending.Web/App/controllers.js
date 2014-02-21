'use strict';

/* Controllers */

var lendtomeControllers = angular.module('lendtomeControllers', []);

lendtomeControllers.controller('userItemsController', ['$scope', 'userItems',
  function ($scope, userItems) {
      $scope.userItems = userItems.query();

      $scope.addNew = function() {
          userItems.save($scope.newUserItem);
      };

  }]);