lendtomeServices.factory('searchBook', [
    '$resource', 'SatellizerStorage',
    function ($resource, storage) {
        return $resource('/api/books', { libraryId: storage.get('userId') }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeControllers.controller('searchBooksController', [
    '$scope', 'searchBook', '$resource',
    function ($scope, searchBooks, $resource) {

        $scope.search = function (searchTerm) {
            var bookSearch = $resource('/api/books/:searchTerm', { searchTerm: '@searchTerm' });
            bookSearch.query({ searchTerm: searchTerm }, function (data) {
                $scope.searchResults = data;
            });
        };

    }
]);