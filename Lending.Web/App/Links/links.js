lendtomeServices.factory('link', [
    '$resource', 'SatellizerStorage',
    function ($resource, storage) {
        return $resource('/api/libraries/:libraryId/links/', { libraryId: storage.get('userId') }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeControllers.controller('linksController', [
    '$scope', 'link', '$location', '$resource', 'SatellizerStorage',
    function($scope, link, $location, $resource, storage) {
        $scope.links = link.query();

        $scope.search = function(searchTerm) {
            var librarySearch = $resource('/api/libraries/:searchTerm', { searchTerm: '@searchTerm' });
            librarySearch.query({ searchTerm: searchTerm }, function(data) {
                $scope.searchResults = data;
            });
        };

        $scope.requestLink = function(targetLibraryId) {
            var linkRequest = $resource('/api/libraries/:libraryId/links/request', { libraryId: storage.get('userId') });
            linkRequest.save({
                TargetLibraryId: targetLibraryId
            }, function(data, error) {

            });
        }
    }
]);