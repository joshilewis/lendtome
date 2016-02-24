lendtomeServices.factory('library', [
    '$resource',
    function ($resource) {
        return $resource('/api/libraries/:libraryId/', { libraryId: '@id' }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeServices.factory('book', [
    '$resource',
    function ($resource) {
        return $resource('/api/libraries/:libraryId/books/', { libraryId: '@id' }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeControllers.controller('booksController', ['$scope', 'library', '$location', 'book',
  function ($scope, library, $location, book) {
      $scope.libraries = library.query();
      $scope.books = book.query();
      $scope.zxaddress = 'zxing://scan/?ret=' + window.location.protocol + '//' + window.location.host + '/addisbn/{CODE}';

      $scope.openLibrary = function (libraryName) {
          var command = { Name: libraryName };
          library.save(command, function () {
              $location('/books');
          });
      }

      $scope.addFromIsbn = function (isbnNumber) {
          $location.path('/addisbn/' + isbnNumber);
      };
  }]);