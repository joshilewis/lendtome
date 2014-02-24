'use strict';

/* Services */

var lendtomeServices = angular.module('lendtomeServices', ['ngResource']);

lendtomeServices.factory('userItems', [
    '$resource',
    function($resource) {
        return $resource('/api/user/items/:ownershipId/', { ownershipId: '@id' }, {
            query: { method: 'GET', params: {}, isArray: true }
        });
    }
]);

lendtomeServices.factory('googleBookDetails', [
    '$resource',
    function ($resource) {
        return $resource('https://www.googleapis.com/books/v1/volumes?q=isbn::isbnNumber', { }, {
        });
    }
]);

