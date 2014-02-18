'use strict';

/* Services */

var lendtomeServices = angular.module('lendtomeServices', ['ngResource']);

lendtomeServices.factory('userItemsService', [
    '$resource',
    function($resource) {
        return $resource('api/user/items/', {}, {
            query: { method: 'GET', params: {}, isArray: false }
        });
    }
]);