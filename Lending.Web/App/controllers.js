'use strict';

/* Controllers */

var lendtomeControllers = angular.module('lendtomeControllers', []);

lendtomeControllers.controller('userItemsController', ['$scope', 'userItemsService',
  function ($scope, userItemsService) {
      $scope.userItems = userItemsService.query();

      $scope.addNew = function() {
          userItemsService.save();
      };

  }]);