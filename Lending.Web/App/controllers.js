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

lendtomeControllers.controller('authController', ['$scope', '$auth', 'SatellizerStorage', '$location',
  function ($scope, $auth, storage, $location) {

      $scope.libraries = 

      $scope.authenticate = function (provider) {
          $auth.authenticate(provider).then(function() {
              var payload = $auth.getPayload();
              storage.set('userName', payload.Claims[0].Value);
              storage.set('userId', payload.Claims[1].Value);
              $location.path('/');
          });
      };


  }]);