(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlSearchForm', wlSearchForm);

    function wlSearchForm() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-search-form.html',
            replace: true,
            controller: function ($scope, $location, userService) {
                $scope.loggedIn = userService.loggedIn();
                $scope.userProfileLink = 'users/' + userService.loggedInUser();

                $scope.searchPhrase = null;
                $scope.searchType = 'Blogs';

                $scope.search = search;

                $scope.$on('$routeChangeSuccess', function () {
                    $scope.searchType = $location.path().includes('post') ? 'Posts' : 'Blogs';
                });

                function search(form) {
                    if (form.$valid) {
                        if ($scope.searchType === 'Blogs') {
                            $location.url($scope.userProfileLink + '/blogs?q=' + $scope.searchPhrase);
                        } else {
                            $location.url($scope.userProfileLink + '/posts?q=' + $scope.searchPhrase);
                        }
                    }
                }
            }
        };
    }

})();