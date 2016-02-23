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
        return $resource('https://www.googleapis.com/books/v1/volumes?q=isbn::isbnNumber',
        {
            maxResults: '10',
            callback: 'JSON_CALLBACK',
            key: 'AIzaSyDCQ090RphWKV_lBNzq7nthXZRo6Q8QvmU',
            fields: 'kind,items(volumeInfo/title, volumeInfo/imageLinks, volumeInfo/authors, volumeInfo/publishedDate)'
        },
        {
            get: {method: 'JSONP'}
        });
    }
]);

lendtomeServices.factory('library', [
    '$resource',
    function ($resource) {
        return $resource('/api/libraries/:libraryId/', {libraryId : '@id'}, {
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



