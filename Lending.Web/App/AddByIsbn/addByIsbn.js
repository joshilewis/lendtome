lendtomeServices.factory('googleBookDetails', [
    '$resource',
    function ($resource) {
        return $resource('https://www.googleapis.com/books/v1/volumes?q=isbn::isbnNumber',
        {
            maxResults: '10',
            callback: 'JSON_CALLBACK',
            key: 'AIzaSyDCQ090RphWKV_lBNzq7nthXZRo6Q8QvmU',
            fields: 'kind,items(volumeInfo/title, volumeInfo/imageLinks, volumeInfo/authors, volumeInfo/publishedDate, volumeInfo/industryIdentifiers)'
        },
        {
            get: { method: 'JSONP' }
        });
    }
]);

lendtomeServices.factory('addBook', [
    '$resource', 'SatellizerStorage',
    function ($resource, storage) {
        return $resource('/api/libraries/:libraryId/books/add', { libraryId: storage.get('userId') });
    }
]);


lendtomeControllers.controller('addByIsbnController', ['$scope', 'googleBookDetails', '$routeParams', 'addBook', '$location',
  function ($scope, bookDetails, $routeParams, addBook, $location) {
      $scope.isbnNumber = $routeParams.isbnnumber;
      $scope.bookDetails = bookDetails.get({ isbnNumber: $scope.isbnNumber });

      $scope.addNew = function () {
          var bookToAdd = {
              Title: $scope.bookDetails.items[0].volumeInfo.title,
              Author: $scope.bookDetails.items[0].volumeInfo.authors[0],
              PublishYear: $scope.bookDetails.items[0].volumeInfo.publishedDate,
              Isbn: $scope.bookDetails.items[0].volumeInfo.industryIdentifiers[1].identifier
          };
          addBook.save(bookToAdd);
          $location.path('/books');
      };


  }]);

