(function () {

    'use strict';

    angular
        .module('app')
        .factory('hateoasService', [hateoasService]);

    function hateoasService() {
        return {
            getBlogsByUserLink: getBlogsByUserLink
        };

        function getBlogsByUserLink(userLinks) {
            var apiLink = userLinks.find(l => l.rel === 'getBlogsByUser').href;

            return apiLink.substr(apiLink.lastIndexOf("users"));
        }
    }

})();