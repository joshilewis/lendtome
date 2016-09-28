lendtomeServices.factory('sentRequest', [
    '$resource', 'SatellizerStorage',
    function ($resource, storage) {
        return $resource('/api/libraries/:libraryId/links/sent', { libraryId: storage.get('userId') }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeControllers.controller('sentRequestsController', [
    '$scope', 'sentRequest',
    function ($scope, sentRequests) {
        $scope.sentRequests = sentRequests.query();

    }
]);