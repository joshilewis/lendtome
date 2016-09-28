lendtomeServices.factory('receivedRequest', [
    '$resource', 'SatellizerStorage',
    function ($resource, storage) {
        return $resource('/api/libraries/:libraryId/links/received', { libraryId: storage.get('userId') }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeControllers.controller('receivedRequestsController', [
    '$scope', 'receivedRequest', '$resource', 'SatellizerStorage',
    function ($scope, receivedRequests, $resource, storage) {
        $scope.receivedRequests = receivedRequests.query();

        $scope.acceptLink= function (requestingLibraryId) {
            var acceptLinkRequest = $resource('/api/libraries/:libraryId/links/accept', { libraryId: storage.get('userId') });
            acceptLinkRequest.save({
                RequestingLibraryId: requestingLibraryId
            }, function (data, error) {

            });
        }
    }

]);