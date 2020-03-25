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
            controller: function ($scope) {
                $scope.previousPageLink = null;
                $scope.nextPageLink = null;

                $scope.pages = [];

                $scope.$watch(
                    function () {
                        return $scope.pageInfo.totalPages;
                    },
                    function (newValue, oldValue) {
                        if (newValue) {
                            generatePageLinks($scope.pageInfo);
                        }
                    });

                function generatePageLinks(pageInfo) {
                    if (pageInfo.totalPages !== 0) {
                        if (pageInfo.currentPage !== 1) {
                            $scope.previousPageLink = $scope.currentUrl +
                                'pageNumber=' + (pageInfo.currentPage - 1);
                        }
                        if (pageInfo.currentPage !== pageInfo.totalPages) {
                            $scope.nextPageLink = $scope.currentUrl +
                                'pageNumber=' + (pageInfo.currentPage + 1);
                        }
                    }

                    for (var i = 1; i <= pageInfo.totalPages; i++) {
                        $scope.pages.push({
                            link: $scope.currentUrl + 'pageNumber=' + i,
                            number: i,
                            current: (i === pageInfo.currentPage)
                        })
                    }
                }
            }
        };
    }

})();