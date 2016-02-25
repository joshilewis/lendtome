lendtomeServices.factory('library', [
    '$resource',
    function ($resource) {
        return $resource('/api/libraries/:libraryId/', { libraryId: '@id' }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeServices.factory('book', [
    '$resource','SatellizerStorage',
    function ($resource, storage) {
        return $resource('/api/libraries/:libraryId/books/', { libraryId: storage.get('userId') }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeServices.factory('deleteBook', [
    '$resource', 'SatellizerStorage',
    function ($resource, storage) {
        return $resource('/api/libraries/:libraryId/books/delete', { libraryId: storage.get('userId') });
    }
]);

lendtomeControllers.controller('booksController', [
    '$scope', 'library', '$location', 'book', '$resource', 'SatellizerStorage',
    function ($scope, library, $location, book, $resource, storage) {
        $scope.libraries = library.query();
        $scope.books = book.query();

        $scope.zxaddress = 'zxing://scan/?ret=' + window.location.protocol + '//' + window.location.host + '/addisbn/{CODE}';

        $scope.openLibrary = function(libraryName) {
            var command = { Name: libraryName };
            library.save(command, function() {
                $location.path('/books');
            });
        }

        $scope.addFromIsbn = function(isbnNumber) {
            $location.path('/addisbn/' + isbnNumber);
        };

        $scope.removeBook = function(title, author, isbnNumber, publishYear) {
            $resource('/api/libraries/:libraryId/books/remove', { libraryId: storage.get('userId') })
                .save({
                    Title: title,
                    Author: author,
                    Isbn: isbnNumber,
                    PublishYear: publishYear
                }, function() {
                    $location.path('/books');
                });
        }
    }
]);