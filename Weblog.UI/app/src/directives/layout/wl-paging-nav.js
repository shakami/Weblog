(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlPagingNav', wlPagingNav);

    function wlPagingNav() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-paging-nav.html',
            scope: {
                currentUrl: '=',
                pageInfo: '='
            },
            controller: function ($scope, $timeout) {
                $scope.previousPageLink = null;
                $scope.nextPageLink = null;

                $scope.pages = [];

                $timeout(function () {
                    generatePageLinks($scope.pageInfo);
                }, 250);

                function generatePageLinks(pageInfo) {
                    if (pageInfo.currentPage !== 1) {
                        $scope.previousPageLink = $scope.currentUrl +
                            '?pageNumber=' + (pageInfo.currentPage - 1);
                    }
                    if (pageInfo.totalPages !== 0 && pageInfo.currentPage !== pageInfo.totalPages) {
                        $scope.nextPageLink = $scope.currentUrl +
                            '?pageNumber=' + (pageInfo.currentPage + 1);
                    }

                    for (var i = 1; i <= pageInfo.totalPages; i++) {
                        $scope.pages.push({
                            link: $scope.currentUrl + '?pageNumber=' + i,
                            number: i,
                            current: (i === pageInfo.currentPage)
                        })
                    }
                }
            }
        };
    }

})();