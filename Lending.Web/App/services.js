'use strict';

/* Services */

var lendtomeServices = angular.module('lendtomeServices', ['ngResource']);

lendtomeServices.factory('userItems', [
    '$resource',
    function($resource) {
        return $resource('api/user/items/:ownershipId/', { ownershipId: '@id' }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);