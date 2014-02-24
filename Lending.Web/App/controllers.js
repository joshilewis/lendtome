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

lendtomeControllers.controller('googleBooksController', ['$scope', 'googleBookDetails', '$routeParams', 'userItems', '$location',
  function ($scope, bookDetails, $routeParams, userItems, $location) {
      $scope.isbnNumber = $routeParams.isbnnumber;
      $scope.bookDetails = bookDetails.get({ isbnNumber: $scope.isbnNumber });

      $scope.addNew = function () {
          $scope.userItem = {
              title: $scope.bookDetails.items[0].volumeInfo.title,
              creator: $scope.bookDetails.items[0].volumeInfo.authors[0],
              edition: $scope.bookDetails.items[0].volumeInfo.publishedDate
          };
          userItems.save($scope.userItem);
          $location.path('/myitems/');
      };


  }]);
